﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;

namespace Blazor.SourceGenerators.Builders;

[DebuggerDisplay("{ToSourceCodeString()}", Name = "{_options.TypeName}")]
internal sealed class SourceBuilder
{
    private const string _newLine = "\r\n";
    private const string _twoNewLines = $"{_newLine}{_newLine}";

    private readonly StringBuilder _builder = new();
    private readonly GeneratorOptions _options;

    private Indentation _indentation = new(0);
    private string? _implementationName;
    private string? _interfaceName;

    internal int IndentationLevel => _indentation.Level;
    internal string ImplementationName => _implementationName ??= _options.Implementation!.ToImplementationName();
    internal string InterfaceName => _interfaceName ??= $"I{_options.TypeName}";

    internal SourceBuilder(GeneratorOptions options) => _options = options;

    internal SourceBuilder AppendCopyRightHeader()
    {
        _builder.Append($"// Copyright (c) David Pine. All rights reserved.{_newLine}");
        _builder.Append($"// Licensed under the MIT License:{_newLine}");
        _builder.Append($"// https://github.com/IEvangelist/blazorators/blob/main/LICENSE{_newLine}");
        _builder.Append($"// Auto-generated by blazorators.{_twoNewLines}");

        return this;
    }

    internal SourceBuilder AppendUsingDeclarations()
    {
        if (_options.SupportsGenerics)
        {
            _builder.Append($"using Blazor.Serialization.Extensions;{_newLine}");
            _builder.Append($"using System.Text.Json;{_newLine}");
        }

        if (!_options.IsWebAssembly)
        {
            _builder.Append($"using System.Threading.Tasks;{_newLine}");
        }

        _builder.Append(_newLine);

        return this;
    }

    internal SourceBuilder AppendNamespace(string namespaceString, bool isNullableContext = true)
    {
        if (isNullableContext)
        {
            _builder.Append($"#nullable enable{_newLine}");
        }

        _builder.Append($"namespace {namespaceString};{_twoNewLines}");

        return this;
    }

    internal SourceBuilder AppendStaticPartialClassDeclaration(
        string className, string? accessModifier)
    {
        _builder.Append($"{accessModifier ?? "internal"} static partial class {className}{_newLine}");

        return this;
    }

    internal SourceBuilder AppendPublicInterfaceDeclaration()
    {
        _builder.Append($"/// <summary>{_newLine}");
        _builder.Append($"/// Source generated interface definition of the <c>{_options.TypeName}</c> type.{_newLine}");
        _builder.Append($"/// </summary>{_newLine}");
        _builder.Append($"public interface I{_options.TypeName}{_newLine}");

        return this;
    }

    internal SourceBuilder AppendInternalImplementationDeclaration()
    {
        _builder.Append($"/// <inheritdoc />{_newLine}");
        _builder.Append($"internal class {ImplementationName} : {InterfaceName}{_newLine}");

        return this;
    }

    internal SourceBuilder AppendImplementationCtor()
    {
        var javaScriptRuntime = _options.IsWebAssembly
            ? "IJSInProcessRuntime"
            : "IJSRuntime";

        _builder.Append($"{_indentation}private readonly {javaScriptRuntime} _javaScript = null!;{_twoNewLines}");
        _builder.Append($"{_indentation}public {ImplementationName}({javaScriptRuntime} javaScript) =>{_newLine}");

        IncreaseIndentation();

        _builder.Append($"{_indentation}_javaScript = javaScript;{_twoNewLines}");

        DecreaseIndentation();

        return this;
    }

    internal SourceBuilder AppendOpeningCurlyBrace(bool increaseIndentation = false)
    {
        IncreaseIndentationImpl(increaseIndentation);

        _builder.Append($"{_indentation}{{{_newLine}");

        return this;
    }

    internal SourceBuilder AppendClosingCurlyBrace(bool decreaseIndentation = false)
    {
        DecreaseIndentationImpl(decreaseIndentation);

        _builder.Append($"{_indentation}}}{_newLine}");

        return this;
    }

    internal SourceBuilder AppendTripleSlashMethodComments(
        CSharpMethod method,
        bool extrapolateParameters = false,
        IndentationAdjustment adjustment = IndentationAdjustment.Noop)
    {
        AdjustIndentation(adjustment);
        var indent = _indentation.ToString();

        _builder.Append($"{indent}/// <summary>{_newLine}");

        var jsMethodName = method.RawName.LowerCaseFirstLetter();
        var func = $"{_options.Implementation}.{jsMethodName}";

        _builder.Append($"{indent}/// Source generated implementation of <c>{func}</c>.{_newLine}");
        var rootUrl = "https://developer.mozilla.org/docs/Web/API";
        var fullUrl = $"{rootUrl}/{_options.TypeName}/{jsMethodName}";
        _builder.Append($"{indent}/// <a href=\"{fullUrl}\"></a>{_newLine}");
        _builder.Append($"{indent}/// </summary>{_newLine}");

        if (extrapolateParameters)
        {
            foreach (var (index, param) in method.ParameterDefinitions.Select())
            {
                if (index.IsFirst)
                {
                    _builder.Append(
                        $"/// <param name=\"component\">The calling Razor (or Blazor) component.</param>{_newLine}");
                }

                if (param.ActionDeclation is not null)
                {
                    var name = param.ToArgumentString();
                    var dependentTypes = param.ActionDeclation.DependentTypes.Keys;
                    var action =
                        $"Expects the name of a <c>\"JSInvokableAttribute\"</c> C# method with the following " +
                        $"<c>System.Action{{{string.Join(", ", dependentTypes)}}}\"</c>.";
                    _builder.Append(
                        $"/// <param name=\"{name}\">{action}</param>{_newLine}");
                }
                else
                {
                    _builder.Append(
                        $"/// <param name=\"{param.RawName}\">The <c>{param.RawTypeName}</c> value.</param>{_newLine}");
                }
            }
        }

        return this;
    }

    internal SourceBuilder AppendEmptyTripleSlashInheritdocComments(
        IndentationAdjustment adjustment = IndentationAdjustment.Noop)
    {
        AdjustIndentation(adjustment);
        var indent = _indentation.ToString();

        _builder.Append($"{indent}/// <inheritdoc />{_newLine}");

        return this;
    }

    internal SourceBuilder AppendTripleSlashInheritdocComments(
        string csharpTypeName,
        string memberName,
        IndentationAdjustment adjustment = IndentationAdjustment.Noop)
    {
        AdjustIndentation(adjustment);
        var indent = _indentation.ToString();

        _builder.Append($"{indent}/// <inheritdoc cref=\"{csharpTypeName}.{memberName}\" />{_newLine}");

        return this;
    }

    internal SourceBuilder AppendTripleSlashPropertyComments(
        CSharpProperty property,
        IndentationAdjustment adjustment = IndentationAdjustment.Noop)
    {
        AdjustIndentation(adjustment);
        var indent = _indentation.ToString();

        _builder.Append($"{indent}/// <summary>{_newLine}");

        var jsMethodName = property.RawName.LowerCaseFirstLetter();
        var func = $"{_options.Implementation}.{jsMethodName}";

        _builder.Append($"{indent}/// Source generated implementation of <c>{func}</c>.\r\n");
        var rootUrl = "https://developer.mozilla.org/docs/Web/API";
        var fullUrl = $"{rootUrl}/{_options.TypeName}/{jsMethodName}";
        _builder.Append($"{indent}/// <a href=\"{fullUrl}\"></a>\r\n");
        _builder.Append($"{indent}/// </summary>\r\n");

        return this;
    }

    internal SourceBuilder AppendLine()
    {
        // We use a hard-coded new line instead of:
        // _builder.AppendLine() as the new line value changes by environment.
        // For consistency, we'll always generate the exact same new line.
        _builder.Append(_newLine);

        return this;
    }

    internal SourceBuilder AppendRaw(
        string content,
        bool appendNewLine = true,
        bool postIncreaseIndentation = false,
        bool omitIndentation = false)
    {
        var indentation = omitIndentation ? "" : _indentation.ToString();
        _builder.Append($"{indentation}{content}{(appendNewLine ? _newLine : string.Empty)}");

        if (postIncreaseIndentation)
        {
            IncreaseIndentation();
        }

        return this;
    }

    internal SourceBuilder IncreaseIndentation()
    {
        IncreaseIndentationImpl(true);

        return this;
    }

    internal SourceBuilder DecreaseIndentation()
    {
        DecreaseIndentationImpl(true);

        return this;
    }

    internal SourceBuilder ResetIndentiationTo(int level)
    {
        _indentation = _indentation.ResetTo(level);

        return this;
    }

    private void IncreaseIndentationImpl(bool increaseIndentation = false) =>
        AdjustIndentation(increaseIndentation
            ? IndentationAdjustment.Increase
            : IndentationAdjustment.Noop);

    private void DecreaseIndentationImpl(bool decreaseIndentation = false) =>
        AdjustIndentation(decreaseIndentation
            ? IndentationAdjustment.Decrease
            : IndentationAdjustment.Noop);

    private void AdjustIndentation(IndentationAdjustment adjustment) =>
        _indentation = adjustment switch
        {
            IndentationAdjustment.Increase => _indentation.Increase(),
            IndentationAdjustment.Decrease => _indentation.Decrease(),
            _ => _indentation
        };

    internal string ToSourceCodeString() => _builder.ToString();
}
