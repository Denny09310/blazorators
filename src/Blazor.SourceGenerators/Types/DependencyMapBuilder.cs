// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Reflection.Metadata;
using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Types;

internal class DependencyMapBuilder()
{
    private readonly Dictionary<string, DeclarationStatement> _dependencies = [];
    private readonly Queue<Node> _processing = [];

    private TypeDeclarationReader _reader = TypeDeclarationReader.Default;

    public Dictionary<string, DeclarationStatement> Build(string typeName)
    {
        typeName = FormatTypeName(typeName);
        if (!TryGetDeclaration(typeName, out var declaration)) return _dependencies;

        if (_dependencies.ContainsKey(typeName)) return _dependencies;
        _dependencies.Add(typeName, declaration);

        _processing.Enqueue(declaration);

        while (_processing.Count > 0)
        {
            var node = _processing.Dequeue();

            var methods = node.OfKind(TypeScriptSyntaxKind.MethodSignature);
            foreach (var method in methods.Cast<MethodSignature>())
            {
                BuildMethod(method);
            }

            var properties = node.OfKind(TypeScriptSyntaxKind.PropertySignature);
            foreach (var property in properties)
            {
                BuildProperty(property);
            }

            if (node.OfKind(TypeScriptSyntaxKind.CallSignature).FirstOrDefault() is CallSignatureDeclaration callback)
            {
                BuildAction(callback);
            }
        }

        return _dependencies;
    }

    public DependencyMapBuilder WithReader(TypeDeclarationReader reader)
    {
        _reader = reader;
        return this;
    }

    private static string FormatTypeName(string typeName)
    {
        var nonGenericMethodReturnType = typeName.ExtractGenericType();
        return nonGenericMethodReturnType.Replace("[]", "");
    }

    private static string GetNodeText(INode node)
    {
        return node.GetText().ToString().Trim();
    }

    private void BuildAction(CallSignatureDeclaration callback)
    {
        var parameterTypes = callback.Parameters.OfKind(TypeScriptSyntaxKind.TypeReference);
        foreach (var type in parameterTypes.Cast<TypeReferenceNode>())
        {
            Build(GetNodeText(type));
        }

        var returnTypes = callback.OfKind(TypeScriptSyntaxKind.TypeReference);
        foreach (var type in returnTypes)
        {
            Build(GetNodeText(type));
        }
    }

    private void BuildMethod(MethodSignature method)
    {
        foreach (var parameter in method.Parameters)
        {
            var parameterTypes = parameter.OfKind(TypeScriptSyntaxKind.TypeReference);
            foreach (var type in parameterTypes.Cast<TypeReferenceNode>())
            {
                Build(GetNodeText(type));
            }
        }

        var returnTypes = method.OfKind(TypeScriptSyntaxKind.TypeReference);
        foreach (var type in returnTypes)
        {
            Build(GetNodeText(type));
        }
    }

    private void BuildProperty(Node property)
    {
        var propertyTypes = property.OfKind(TypeScriptSyntaxKind.TypeReference);
        foreach (var type in propertyTypes)
        {
            Build(GetNodeText(type));
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