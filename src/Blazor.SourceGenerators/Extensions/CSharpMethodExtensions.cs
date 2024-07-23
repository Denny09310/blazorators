﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.Builders;
using Blazor.SourceGenerators.Options;

namespace Blazor.SourceGenerators.Extensions;

internal static class CSharpMethodExtensions
{
    internal static (string ReturnType, string BareType) GetMethodTypes(this CSharpMethod method, GeneratorOptions options, bool isGenericReturnType, bool isPrimitiveType)
    {
        var primitiveType = isPrimitiveType
            ? Primitives.Instance[method.RawReturnTypeName]
            : method.RawReturnTypeName;

        if (method.IsAsync)
        {
            primitiveType = primitiveType.ExtractGenericType();
        }

        if (!method.IsVoid && isGenericReturnType)
        {
            var nullable =
                method.IsReturnTypeNullable ? "?" : "";

            return options.IsWebAssembly
                ? ($"{MethodBuilderDetails.GenericTypeValue}{nullable}", primitiveType)
                : ($"ValueTask<{MethodBuilderDetails.GenericTypeValue}{nullable}>", primitiveType);
        }

        var isJavaScriptOverride = method.IsJavaScriptOverride(options);
        if (options.IsWebAssembly && !isJavaScriptOverride)
        {
            string returnType;
            if (isPrimitiveType)
            {
                returnType = primitiveType;
            }
            else
            {
                returnType = method switch
                {
                    { IsVoid: true, IsAsync: false } => "void",
                    { IsVoid: true, IsAsync: true } => "ValueTask",
                    { IsVoid: false, IsAsync: true } => GetGenericValueTask(method),
                    _ => method.RawReturnTypeName
                };
            }

            return (returnType, primitiveType);
        }
        else
        {
            string returnType;
            if (isPrimitiveType)
            {
                returnType = $"ValueTask<{primitiveType}>";
            }
            else
            {
                returnType = method switch
                {
                    { IsVoid: true, IsAsync: false } => "ValueTask",
                    { IsVoid: true, IsAsync: true } => "ValueTask",
                    { IsVoid: false, IsAsync: true } => GetGenericValueTask(method),
                    _ => $"ValueTask<{method.RawReturnTypeName}>"
                };
            }

            return (returnType, primitiveType);
        }
    }

    internal static bool IsGenericReturnType(this CSharpMethod method, GeneratorOptions options) =>
        Array.Exists(options.GenericMethodDescriptors ?? [], descriptor =>
        {
            // If the descriptor describes a parameter, it's not a generic return.
            // TODO: consider APIs that might do this.
            if (descriptor.Contains(":"))
            {
                return false;
            }

            // If the descriptor is the method name
            return descriptor == method.RawName;
        });

    internal static bool IsJavaScriptOverride(this CSharpMethod method, GeneratorOptions options)
    {
        var methodName = method.RawName.LowerCaseFirstLetter();
        return Array.Exists(options.PureJavaScriptOverrides ?? [], overriddenMethodName => overriddenMethodName == methodName);
    }

    private static string GetGenericValueTask(CSharpMethod method)
    {
        var genericType = method.RawReturnTypeName.ExtractGenericType();
        return Primitives.IsPrimitiveType(genericType)
            ? $"ValueTask<{Primitives.Instance[genericType]}>"
            : $"ValueTask<{genericType}>";
    }
}