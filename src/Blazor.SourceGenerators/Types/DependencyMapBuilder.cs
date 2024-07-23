// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Types;

internal class DependencyMapBuilder()
{
    private static readonly ConcurrentDictionary<string, Dependency> _cache = [];

    private readonly HashSet<string> _found = [];
    private readonly Queue<Node> _processing = [];

    private TypeDeclarationReader _reader = TypeDeclarationReader.Default;

    public Dependency Root { get; private set; }

    public void Build(string typeName)
    {
        if (_cache.TryGetValue(typeName, out var root))
        {
            Root = root;
        }
        else
        {
            Root = BuildInternal(typeName);

            _cache.TryAdd(typeName, Root);

            _found.Clear();
            _processing.Clear();
        }
    }

    public DependencyMapBuilder WithReader(TypeDeclarationReader reader)
    {
        _reader = reader;
        return this;
    }

    private static string FormatName(string typeName)
    {
        // TODO: Probably we should get the arrays removing the formatting from the name, same for the generic types

        var nonGenericMethodReturnType = typeName.ExtractGenericType();
        return nonGenericMethodReturnType.Replace("[]", "");
    }

    private static string GetNodeText(INode node)
    {
        return node.GetText().ToString().Trim();
    }

    private Dependency BuildInternal(string typeName)
    {
        typeName = FormatName(typeName);
        if (_found.Contains(typeName)) return default;

        // FIXME: For now avoid parsing EventMaps
        if (typeName.EndsWith("EventMap")) return default;

        if (!TryGetDeclaration(typeName, out var declaration)) return default;
        var dependency = new Dependency(typeName, declaration, []);

        _found.Add(typeName);
        _processing.Enqueue(declaration);

        while (_processing.Count > 0)
        {
            var node = _processing.Dequeue();

            // Check if has methods
            var methods = node.OfKind(TypeScriptSyntaxKind.MethodSignature);
            foreach (var method in methods.Cast<MethodSignature>())
            {
                IEnumerable<Node> types;

                foreach (var parameter in method.Parameters)
                {
                    types = parameter.OfKind(TypeScriptSyntaxKind.TypeReference);
                    foreach (var type in types)
                    {
                        Resolve(dependency, type);
                    }
                }

                types = method.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(dependency, type);
                }
            }

            // Check if has properties
            var properties = node.OfKind(TypeScriptSyntaxKind.PropertySignature);
            foreach (var property in properties)
            {
                var types = property.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(dependency, type);
                }
            }

            // Check if is a callback type
            if (node.OfKind(TypeScriptSyntaxKind.CallSignature).FirstOrDefault() is CallSignatureDeclaration callback)
            {
                IEnumerable<Node> types;

                foreach (var parameter in callback.Parameters)
                {
                    types = parameter.OfKind(TypeScriptSyntaxKind.TypeReference);
                    foreach (var type in types)
                    {
                        Resolve(dependency, type);
                    }
                }

                types = callback.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(dependency, type);
                }
            }

            // Check if it is a union type
            if (node.OfKind(TypeScriptSyntaxKind.UnionType).FirstOrDefault() is UnionTypeNode union)
            {
                foreach (var type in union.Types.Where(type => type is TypeReferenceNode))
                {
                    Resolve(dependency, type);
                }
            }

            // Check if it is a intersection type
            if (node.OfKind(TypeScriptSyntaxKind.UnionType).FirstOrDefault() is IntersectionTypeNode intersection)
            {
                foreach (var type in intersection.Types.Where(type => type is TypeReferenceNode))
                {
                    Resolve(dependency, type);
                }
            }

            // Check if it is an array type
            if (node.OfKind(TypeScriptSyntaxKind.ArrayType).FirstOrDefault() is ArrayTypeNode array)
            {
                Resolve(dependency, array.ElementType);
            }
        }

        return dependency;
    }

    private void Resolve(Dependency parent, INode type)
    {
        var children = BuildInternal(GetNodeText(type));
        if (children != default)
        {
            parent.Dependencies.Add(children);
        }
    }

    private bool TryGetDeclaration(string typeName, out DeclarationStatement declaration)
    {
        declaration = default!;

        if (Primitives.IsPrimitiveType(typeName)) return false;

        var success = _reader.TryGetInterface(typeName, out var @interface) && @interface is not null;
        if (success)
        {
            declaration = @interface!;
            return true;
        }

        success = _reader.TryGetTypeAlias(typeName, out var @type) && @type is not null;
        if (success)
        {
            declaration = @type!;
            return true;
        }

        return false;
    }
}

public readonly record struct Dependency(
    string Identifier,
    DeclarationStatement Declaration,
    ICollection<Dependency> Dependencies);

public static class DependencyExtensions
{
    public static Dictionary<string, DeclarationStatement> ToDictionary(this Dependency root)
    {
        var result = new Dictionary<string, DeclarationStatement>();
        FlattenDependencies(root, result);
        return result;
    }

    private static void FlattenDependencies(Dependency dependency, Dictionary<string, DeclarationStatement> dictionary)
    {
        if (dictionary.ContainsKey(dependency.Identifier)) return;

        dictionary[dependency.Identifier] = dependency.Declaration;

        foreach (var dep in dependency.Dependencies)
        {
            FlattenDependencies(dep, dictionary);
        }
    }
}
