// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Parsers;

internal sealed partial class TypeDeclarationParser
{
    internal static CSharpObject ToObject(Dependency root)
    {
        var (identifier, declaration, dependencies) = root;

        var heritage = declaration is InterfaceDeclaration @interface ? @interface.HeritageClauses?
                .SelectMany(heritage => heritage.Types)
                .Where(type => type.Identifier is not "EventTarget")
                .Select(type => type.Identifier)
                .ToArray() : null;

        var subclass = heritage is null || heritage.Length == 0 ? "" : string.Join(", ", heritage);

        var csharpObject = new CSharpObject(identifier, subclass);

        var objectMethods = declaration.OfKind(TypeScriptSyntaxKind.MethodSignature);
        var methods = ParseMethods(root, objectMethods);

        csharpObject = csharpObject with
        {
            Methods = methods
                .GroupBy(method => method.RawName)
                .ToDictionary(method => method.Key, method => method.Last())
        };

        var objectProperties = declaration.OfKind(TypeScriptSyntaxKind.PropertySignature);
        var properties = ParseProperties(objectProperties);

        csharpObject = csharpObject with
        {
            Properties = properties
                .GroupBy(property => property.RawName)
                .ToDictionary(property => property.Key, method => method.Last())
        };

        foreach (var dependency in dependencies)
        {
            csharpObject.DependentTypes.Add(dependency.Identifier, ToObject(dependency));
        }

        return csharpObject;
    }

    internal static CSharpTopLevelObject ToTopLevelObject(Dependency root)
    {
        var (identifier, declaration, dependencies) = root;
        var csharpTopLevelObject = new CSharpTopLevelObject(identifier);

        var topLevelObjectMethods = declaration.OfKind(TypeScriptSyntaxKind.MethodSignature);
        var methods = ParseMethods(root, topLevelObjectMethods);

        csharpTopLevelObject.Methods.AddRange(methods);

        var topLevelObjectProperties = declaration.OfKind(TypeScriptSyntaxKind.PropertySignature);
        var properties = ParseProperties(topLevelObjectProperties);

        csharpTopLevelObject.Properties.AddRange(properties);

        foreach (var dependency in dependencies)
        {
            csharpTopLevelObject.DependentTypes.Add(dependency.Identifier, ToObject(dependency));
        }

        return csharpTopLevelObject;
    }

    internal CSharpObject ToObject(string typeName)
    {
        _mapper.Build(typeName);
        if (_mapper.Root != default) return ToObject(_mapper.Root);
        return default!;
    }

    internal CSharpTopLevelObject ToTopLevelObject(string typeName)
    {
        _mapper.Build(typeName);

        if (_mapper.Root != default) return ToTopLevelObject(_mapper.Root);
        return default!;
    }

    private static string CleanseType(string type)
    {
        return type
            .Replace(" | null", "")
            .Replace(" | undefined", "");
    }

    private static string GetNodeText(INode node)
    {
        return node.GetText().ToString().Trim();
    }

    private static bool IsNullableType(string type)
    {
        return type.EndsWith(" | null") || type.EndsWith(" | undefined");
    }

    private static IEnumerable<CSharpMethod> ParseMethods(Dependency root, IEnumerable<Node> objectMethods)
    {
        ICollection<CSharpMethod> methods = [];
        foreach (var method in objectMethods.Cast<MethodSignature>())
        {
            var methodName = method.Identifier;
            var methodParameters = method.Parameters;
            var methodReturnType = GetNodeText(method.Type);

            if (methodName is null || methodParameters is null || string.IsNullOrEmpty(methodReturnType))
            {
                continue;
            }

            // FIXME: For now ignore all method that has "EventListener" in the name
            if (methodName.EndsWith("EventListener"))
            {
                continue;
            }

            var (csharpParameters, javascriptMethod) = ParseParameters(
                root,
                methodName,
                methodParameters);

            var csharpMethod = new CSharpMethod(methodName, methodReturnType, csharpParameters, javascriptMethod);
            methods.Add(csharpMethod);
        }

        return methods;
    }

    private static (IList<CSharpType>, JavaScriptMethod) ParseParameters(Dependency root, string methodName, NodeArray<ParameterDeclaration> methodParameters)
    {
        IList<CSharpType> parameters = [];
        var javascriptMethod = new JavaScriptMethod(methodName);

        foreach (var parameter in methodParameters)
        {
            var parameterName = parameter.Identifier;
            var parameterTypeNode = parameter.Children[parameter.Children.Count - 1];

            // TODO: Handle other type of nodes correctly
            // Examples:
            // -    Rsponse | URL #UnionNodeTypeNode
            // -    ((this: SomeCustom, ev: Event) => any)  #ParenthesizedTypeNode, inside #FunctionTypeNode

            var parameterType = parameterTypeNode switch
            {
                _ => GetNodeText(parameterTypeNode)
            };

            var isNullable = parameter.QuestionToken is not null || IsNullableType(parameterType);

            if (isNullable)
            {
                parameterType = CleanseType(parameterType);
            }

            CSharpAction csharpAction = null!;

            // When a parameter defines a custom type, that type needs to also be parsed
            // and source generated. This is so that dependent types are known and resolved.
            if (!Primitives.IsPrimitiveType(parameterType))
            {
                var dependency = root.Dependencies.FirstOrDefault(dependency => dependency.Identifier == parameterType);
                if (dependency != default)
                {
                    javascriptMethod = javascriptMethod with
                    {
                        InvokableMethodName = $"blazorators.{dependency.Identifier.LowerCaseFirstLetter()}.{methodName}"
                    };

                    if (parameterName.EndsWith("Callback"))
                    {
                        csharpAction = ToAction(dependency);
                        javascriptMethod = javascriptMethod with
                        {
                            IsBiDirectionalJavaScript = true,
                        };
                    }
                }
            }

            parameters.Add(new CSharpType(parameterName, parameterType, isNullable, csharpAction));
        }

        return (parameters, javascriptMethod);
    }

    private static IEnumerable<CSharpProperty> ParseProperties(IEnumerable<Node> objectProperties)
    {
        ICollection<CSharpProperty> properties = [];
        foreach (var property in objectProperties.Cast<PropertySignature>())
        {
            var propertyName = property.Identifier;

            var propertyTypeNode = property.Children[property.Children.Count - 1];

            // TODO: Handle other type of nodes correctly
            // Examples:
            // -    SomeCustomType | null                   #UnionNodeType
            // -    ((this: SomeCustom, ev: Event) => any)  #ParenthesizedTypeNode, inside #FunctionTypeNode

            var propertyType = propertyTypeNode switch
            {
                _ => GetNodeText(propertyTypeNode)
            };

            if (propertyName is null || string.IsNullOrEmpty(propertyType))
            {
                continue;
            }

            var isReadonly = property.Modifiers.Exists(modifier => modifier.Kind is TypeScriptSyntaxKind.ReadonlyKeyword);
            var isNullable = property.QuestionToken is not null || IsNullableType(propertyType);

            if (isNullable)
            {
                propertyType = CleanseType(propertyType);
            }

            // FIXME: For now ignore all properties that starts with "on"
            if (propertyName.StartsWith("on"))
            {
                continue;
            }

            var csharpProperty = new CSharpProperty(propertyName, propertyType, isNullable, isReadonly);
            properties.Add(csharpProperty);
        }

        return properties;
    }

    private static CSharpAction ToAction(Dependency root)
    {
        var (identifier, declaration, dependencies) = root;
        var csharpAction = new CSharpAction(identifier);

        var callSignatureDeclaration = declaration.OfKind(TypeScriptSyntaxKind.CallSignature).FirstOrDefault() as CallSignatureDeclaration;

        if (callSignatureDeclaration is not null)
        {
            var actionParameters = callSignatureDeclaration.Parameters;
            var actionReturnType = GetNodeText(callSignatureDeclaration.Type);

            if (actionParameters is null || string.IsNullOrEmpty(actionReturnType))
            {
                return csharpAction;
            }

            var (csharpParameters, _) = ParseParameters(
                root,
                csharpAction.RawName,
                actionParameters);

            csharpAction = csharpAction with
            {
                ParameterDefinitions = csharpParameters
            };

            foreach (var dependency in dependencies)
            {
                csharpAction.DependentTypes.Add(dependency.Identifier, ToObject(dependency));
            }
        }

        return csharpAction;
    }
}