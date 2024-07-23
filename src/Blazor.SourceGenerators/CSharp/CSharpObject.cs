﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators.CSharp;

/// <summary>
/// A record the represents various C# objects.
/// </summary>
internal record CSharpObject(
    string TypeName,
    string? ExtendsTypeName) : ICSharpDependencyGraphObject
{
    /// <summary>
    /// The collection of types that this object depends on.
    /// </summary>
    public Dictionary<string, CSharpObject> DependentTypes { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public IImmutableSet<DependentType> AllDependentTypes => this.GetAllDependencies()
        .Concat(Properties.SelectMany(p => p.Value.AllDependentTypes))
        .Concat(Methods.SelectMany(p => p.Value.AllDependentTypes))
        .Concat([new DependentType(TypeName, this)])
        .ToImmutableHashSet(DependentTypeComparer.Default);

    /// <summary>
    /// The <see cref="Dictionary{TKey, TValue}.Keys"/> represent the raw parsed member name, while the
    /// corresponding <see cref="Dictionary{TKey, TValue}.Values"/> are the <see cref="CSharpProperty"/> details.
    /// </summary>
    public Dictionary<string, CSharpProperty> Properties { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// The <see cref="Dictionary{TKey, TValue}.Keys"/> represent the raw parsed member name, while the
    /// corresponding <see cref="Dictionary{TKey, TValue}.Values"/> are the <see cref="CSharpMethod"/> details.
    /// </summary>
    public Dictionary<string, CSharpMethod> Methods { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public Dictionary<string, CSharpEnum> EnumValues { get; init; } = new(StringComparer.OrdinalIgnoreCase);

    public bool IsActionParameter =>
        TypeName.EndsWith("Callback");

    public override string ToString()
    {
        StringBuilder builder = new("#nullable enable\r\n");

        builder.Append("using System.Text.Json.Serialization;\r\n\r\n");
        builder.Append("namespace Microsoft.JSInterop;\r\n\r\n");

        builder.Append($"/// <summary>\r\n");
        builder.Append($"/// Source-generated object representing an ideally immutable <c>{TypeName}</c> value.\r\n");
        builder.Append($"/// </summary>\r\n");
        builder.Append($"public class {TypeName}\r\n{{\r\n");

        if (EnumValues.Count > 0)
        {
            BuildConstants(builder);
        }
        else
        {
            BuildProperties(builder);
        }

        builder.Append("}\r\n");
        var result = builder.ToString();
        return result;
    }

    private void BuildConstants(StringBuilder builder)
    {
        foreach (var enumValue in EnumValues)
        {
            var (memberName, member) = (enumValue.Key, enumValue.Value);

            builder.Append($"    /// <summary>\r\n");
            builder.Append($"    /// Source-generated property representing the <c>{TypeName}.{memberName}</c> value.\r\n");
            builder.Append($"    /// </summary>\r\n");
            builder.Append($"    public const string {member.ValueName} = \"{member.RawValue}\";\r\n");  
            builder.Append($"    \r\n");  
        }

        builder.Append($"    [JsonConstructor]\r\n");
        builder.Append($"    public {TypeName}(string value) => Value = value;\r\n\r\n");

        builder.Append($"    public string Value {{ get; set; }}\r\n\r\n");

        builder.Append($"    public static implicit operator string({TypeName} @enum) => @enum.Value;\r\n");
    }

    private void BuildProperties(StringBuilder builder)
    {
        foreach (var (index, property) in Properties.Select((property, index) => (index, property)))
        {
            var (memberName, member) = (property.Key, property.Value);
            var typeName = member.MappedTypeName;
            var nullableExpression = member.IsNullable && !typeName.EndsWith("?") ? "?" : "";
            var trivia = member.IsArray ? "[]" : "";
            var isPrimitive = Primitives.IsPrimitiveType(typeName);
            var statementTerminator = member.IsNullable ||
                typeName is "string" || !isPrimitive ? " = default!;" : "";
            var csharpMemberName = memberName.CapitalizeFirstLetter();

            builder.Append($"    /// <summary>\r\n");
            builder.Append($"    /// Source-generated property representing the <c>{TypeName}.{memberName}</c> value.\r\n");
            builder.Append($"    /// </summary>\r\n");
            builder.Append($"    [JsonPropertyName(\"{memberName}\")]\r\n");
            builder.Append($"    public {typeName}{trivia}{nullableExpression} {csharpMemberName} {{ get; set; }}{statementTerminator}\r\n");

            var isTimestamp = member.RawTypeName is "DOMTimeStamp" or "DOMTimeStamp | null" or "EpochTimeStamp" or "EpochTimeStamp | null";

            if (index <= Properties.Count - 2 || isTimestamp)
            {
                builder.Append($"\r\n");
            }

            // Add readonly property for converting DOMTimeStamp (long) to DateTime.
            if (isTimestamp)
            {
                builder.Append($"    /// <summary>\r\n");
                builder.Append($"    /// Source-generated property representing the <c>{TypeName}.{memberName}</c> value, \r\n");
                builder.Append($"    /// converted as a <see cref=\"System.DateTime\" /> in UTC.\r\n");
                builder.Append($"    /// </summary>\r\n");

                var nullable = member.IsNullable ? "?" : "";
                builder.Append($"    [JsonIgnore]\r\n");
                builder.Append($"    public DateTime{nullable} {csharpMemberName}AsUtcDateTime => {csharpMemberName}.ToDateTimeFromUnix();\r\n");
            }
        }
    }
}