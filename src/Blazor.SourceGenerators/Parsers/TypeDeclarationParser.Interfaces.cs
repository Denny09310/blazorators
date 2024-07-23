// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Parsers;

internal sealed partial class TypeDeclarationParser
{
    internal static CSharpObject ToObject(TypeDescriptor descriptor)
    {
        var (identifier, declaration, _) = descriptor;

        return declaration switch
        {
            TypeAliasDeclaration => FromTypeAlias(descriptor),
            InterfaceDeclaration => FromInterface(descriptor),
            _ => throw new NotImplementedException($"Invalid type declaration '{declaration.GetType().Name}' for '{identifier}'"),
        };
    }

    internal static CSharpTopLevelObject ToTopLevelObject(TypeDescriptor root)
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
        var descriptor = _mapper.Build(typeName);

        if (descriptor != default) return ToObject(descriptor);
        return default!;
    }

    internal CSharpTopLevelObject ToTopLevelObject(string typeName)
    {
        var descriptor = _mapper.Build(typeName);

        if (descriptor != default) return ToTopLevelObject(descriptor);
        return default!;
    }

    private static string CleanseType(string type)
    {
        return type
            .Replace(" | null", "")
            .Replace(" | undefined", "");
    }

    private static CSharpObject FromInterface(TypeDescriptor descriptor)
    {
        var (identifier, declaration, dependencies) = descriptor;

        var heritage = ((InterfaceDeclaration)declaration).HeritageClauses?
                .SelectMany(heritage => heritage.Types)
                .Where(type => type.Identifier is not "EventTarget")
                .Select(type => type.Identifier)
                .ToArray();

        var subclass = heritage is null || heritage.Length == 0 ? "" : string.Join(", ", heritage);

        var csharpObject = new CSharpObject(identifier, subclass);

        var objectMethods = declaration.OfKind(TypeScriptSyntaxKind.MethodSignature);
        var methods = ParseMethods(descriptor, objectMethods);

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

    private static CSharpObject FromTypeAlias(TypeDescriptor descriptor)
    {
        var (identifier, declaration, dependencies) = descriptor;

        var csharpObject = new CSharpObject(identifier, null);

        if (declaration.OfKind(TypeScriptSyntaxKind.UnionType).FirstOrDefault() is UnionTypeNode union)
        {
            foreach (var type in union.Types)
            {
                var typeName = type.GetNodeText();
                var typeDescriptor = dependencies.FirstOrDefault(type => type.Identifier == typeName);

                if (typeDescriptor != default)
                {
                    csharpObject.DependentTypes.Add(typeName, ToObject(typeDescriptor));
                }
                else if (type is LiteralTypeNode)
                {
                    typeName = typeName.Replace("\"", "");
                    csharpObject.EnumValues.Add(typeName, new CSharpEnum(typeName.FormatLiteral(), typeName));
                }
            }
        }

        if (declaration.OfKind(TypeScriptSyntaxKind.IntersectionType).FirstOrDefault() is IntersectionTypeNode intersection)
        {
            foreach (var type in intersection.Types)
            {
                var typeName = type.GetNodeText();
                var typeDescriptor = dependencies.FirstOrDefault(type => type.Identifier == typeName);

                if (typeDescriptor != default)
                {
                    csharpObject.DependentTypes.Add(typeName, ToObject(typeDescriptor));
                }
            }
        }

        if (declaration.OfKind(TypeScriptSyntaxKind.ArrayType).FirstOrDefault() is ArrayTypeNode array)
        {
            var typeName = array.ElementType.GetNodeText();
            var typeDescriptor = dependencies.FirstOrDefault(type => type.Identifier == typeName);

            if (typeDescriptor != default)
            {
                csharpObject.DependentTypes.Add(typeName, ToObject(typeDescriptor));
            }
        }

        return csharpObject;
    }

    private static bool IsNullableType(string type)
    {
        return type.EndsWith(" | null") || type.EndsWith(" | undefined");
    }

    private static IEnumerable<CSharpMethod> ParseMethods(TypeDescriptor descriptor, IEnumerable<Node> objectMethods)
    {
        ICollection<CSharpMethod> methods = [];
        foreach (var method in objectMethods.Cast<MethodSignature>())
        {
            var methodName = method.Identifier;
            var methodParameters = method.Parameters;
            var methodReturnType = method.Type.GetNodeText();

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
                descriptor,
                methodName,
                methodParameters);

            var csharpMethod = new CSharpMethod(methodName, methodReturnType, csharpParameters, javascriptMethod);
            methods.Add(csharpMethod);
        }

        return methods;
    }

    private static (IList<CSharpType>, JavaScriptMethod) ParseParameters(TypeDescriptor descriptor, string methodName, NodeArray<ParameterDeclaration> methodParameters)
    {
        IList<CSharpType> parameters = [];
        var javascriptMethod = new JavaScriptMethod(methodName);

        foreach (var parameter in methodParameters)
        {
            var parameterName = parameter.Identifier;

            // TODO: Handle other type of nodes correctly
            // Examples:
            // -    Rsponse | URL #UnionNodeTypeNode
            // -    ((this: SomeCustom, ev: Event) => any)  #ParenthesizedTypeNode, inside #FunctionTypeNode

            var parameterType = parameter.Last switch
            {
                _ => parameter.Last.GetNodeText()
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
                var dependency = descriptor.Dependencies.FirstOrDefault(dependency => dependency.Identifier == parameterType);
                if (dependency != default)
                {
                    javascriptMethod = javascriptMethod with
                    {
                        InvokableMethodName = $"blazorators.{descriptor.Identifier.LowerCaseFirstLetter()}.{methodName}"
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

            // TODO: Handle other type of nodes correctly
            // Examples:
            // -    SomeCustomType | null                   #UnionNodeType
            // -    ((this: SomeCustom, ev: Event) => any)  #ParenthesizedTypeNode, inside #FunctionTypeNode

            var propertyType = property.Last switch
            {
                _ => property.Last.GetNodeText()
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

    private static CSharpAction ToAction(TypeDescriptor descriptor)
    {
        var (identifier, declaration, dependencies) = descriptor;
        var csharpAction = new CSharpAction(identifier);

        var callSignatureDeclaration = declaration.OfKind(TypeScriptSyntaxKind.CallSignature).FirstOrDefault() as CallSignatureDeclaration;

        if (callSignatureDeclaration is not null)
        {
            var actionParameters = callSignatureDeclaration.Parameters;
            var actionReturnType = callSignatureDeclaration.Type.GetNodeText();

            if (actionParameters is null || string.IsNullOrEmpty(actionReturnType))
            {
                return csharpAction;
            }

            var (csharpParameters, _) = ParseParameters(
                descriptor,
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