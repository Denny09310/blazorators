// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Types;

internal class DependencyMapBuilder()
{
    private static readonly ConcurrentDictionary<string, TypeDescriptor> _cache = [];

    private readonly HashSet<string> _found = [];
    private readonly Queue<Node> _processing = [];

    private TypeDeclarationReader _reader = TypeDeclarationReader.Default;

    public TypeDescriptor Build(string typeName)
    {
        if (_cache.TryGetValue(typeName, out var descriptor))
        {
            return descriptor;
        }

        descriptor = BuildInternal(typeName);
        if (descriptor != default)
        {
            _cache.TryAdd(typeName, descriptor);
        }

        _found.Clear();
        _processing.Clear();

        return descriptor;
    }

    public DependencyMapBuilder WithReader(TypeDeclarationReader reader)
    {
        _reader = reader;
        return this;
    }

    private TypeDescriptor BuildInternal(string typeName)
    {
        typeName = typeName.FormatTypeName();
        if (_found.Contains(typeName)) return default;

        if (typeName.Contains("Event")) return default;

        if (!TryGetDeclaration(typeName, out var declaration)) return default;
        var descriptor = new TypeDescriptor(typeName, declaration, []);

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
                        Resolve(descriptor, type);
                    }
                }

                types = method.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(descriptor, type);
                }
            }

            // Check if has properties
            var properties = node.OfKind(TypeScriptSyntaxKind.PropertySignature);
            foreach (var property in properties)
            {
                var types = property.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(descriptor, type);
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
                        Resolve(descriptor, type);
                    }
                }

                types = callback.OfKind(TypeScriptSyntaxKind.TypeReference);
                foreach (var type in types)
                {
                    Resolve(descriptor, type);
                }
            }

            // Check if it is a union type
            if (node.OfKind(TypeScriptSyntaxKind.UnionType).FirstOrDefault() is UnionTypeNode union)
            {
                foreach (var type in union.Types.Where(type => type is TypeReferenceNode))
                {
                    Resolve(descriptor, type);
                }
            }

            // Check if it is a intersection type
            if (node.OfKind(TypeScriptSyntaxKind.UnionType).FirstOrDefault() is IntersectionTypeNode intersection)
            {
                foreach (var type in intersection.Types.Where(type => type is TypeReferenceNode))
                {
                    Resolve(descriptor, type);
                }
            }

            // Check if it is an array type
            if (node.OfKind(TypeScriptSyntaxKind.ArrayType).FirstOrDefault() is ArrayTypeNode array)
            {
                Resolve(descriptor, array.ElementType);
            }
        }

        return descriptor;
    }

    private void Resolve(TypeDescriptor descriptor, INode type)
    {
        var children = BuildInternal(type.GetNodeText());
        if (children != default)
        {
            descriptor.Dependencies.Add(children);
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

public readonly record struct TypeDescriptor(
    string Identifier,
    DeclarationStatement Declaration,
    ICollection<TypeDescriptor> Dependencies);

public static class TypDescriptorExtensions
{
    public static Dictionary<string, DeclarationStatement> ToDictionary(this TypeDescriptor root)
    {
        var result = new Dictionary<string, DeclarationStatement>();
        FlattenDependencies(root, result);
        return result;
    }

    private static void FlattenDependencies(TypeDescriptor dependency, Dictionary<string, DeclarationStatement> dictionary)
    {
        if (dictionary.ContainsKey(dependency.Identifier)) return;

        dictionary[dependency.Identifier] = dependency.Declaration;

        foreach (var dep in dependency.Dependencies)
        {
            FlattenDependencies(dep, dictionary);
        }
    }
}
