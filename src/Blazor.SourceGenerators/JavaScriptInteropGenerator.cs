﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators;

[Generator]
internal sealed partial class JavaScriptInteropGenerator : ISourceGenerator
{
    private readonly HashSet<(string FileName, string SourceCode)> _sourceCodeToAdd =
    [
        (nameof(RecordCompat).ToGeneratedFileName(), RecordCompat),
        (nameof(BlazorHostingModel).ToGeneratedFileName(), BlazorHostingModel),
        (nameof(JSAutoInteropAttribute).ToGeneratedFileName(), JSAutoInteropAttribute),
        (nameof(JSAutoGenericInteropAttribute).ToGeneratedFileName(), JSAutoGenericInteropAttribute),
    ];

    public void Initialize(GeneratorInitializationContext context)
    {
        // Register a syntax receiver that will be created for each generation pass
        context.RegisterForSyntaxNotifications(JavaScriptInteropSyntaxContextReceiver.Create);
    }

    public void Execute(GeneratorExecutionContext context) => TryExecute(context);

    private void TryExecute(GeneratorExecutionContext context)
    {
        try
        {
            // Add source from text.
            foreach (var (fileName, sourceCode) in _sourceCodeToAdd)
            {
                context.AddSource(fileName, SourceText.From(sourceCode, Encoding.UTF8));
            }

            if (context.SyntaxContextReceiver is not JavaScriptInteropSyntaxContextReceiver receiver)
            {
                return;
            }

            foreach (var (options, classDeclaration, attribute) in receiver.InterfaceDeclarations)
            {
                if (options is null || IsDiagnosticError(options, context, attribute))
                {
                    continue;
                }

                var isPartial = classDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword));
                if (!isPartial)
                {
                    continue;
                }

                var model = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var symbol = model.GetDeclaredSymbol(classDeclaration);
                if (symbol is not ITypeSymbol typeSymbol || typeSymbol.IsStatic)
                {
                    continue;
                }

                foreach (var parser in options.Parsers)
                {
                    var result = parser.ParseTargetType(options.TypeName!);
                    if (result is { Status: ParserResultStatus.SuccessfullyParsed, Value: { } })
                    {
                        var namespaceString = (typeSymbol.ContainingNamespace.ToDisplayString(), classDeclaration.Parent) switch
                        {
                            (string { Length: > 0 } containingNamespace, _) => containingNamespace,
                            (_, BaseNamespaceDeclarationSyntax namespaceDeclaration) => namespaceDeclaration.Name.ToString(),
                            _ => null
                        };

                        var @interface = options.Implementation.ToInterfaceName();
                        var implementation = options.Implementation.ToImplementationName();

                        var topLevelObject = result.Value;
                        context.AddDependentTypesSource(topLevelObject)
                            .AddInterfaceSource(topLevelObject, @interface, options, namespaceString)
                            .AddImplementationSource(topLevelObject, implementation, options, namespaceString)
                            .AddDependencyInjectionExtensionsSource(topLevelObject, implementation, options);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Ensure that we don't entire fail as a source generator.
            // Avoid the red circle that reads:
            //   "error CS8785: Generator 'JavaScriptInteropGenerator' failed to generate source.
            //    It will not contribute to the output and compilation errors may occur as a result."
            //
            // Cleanly propagate the exception as a diagnostic message.
            context.ReportDiagnostic(
                diagnostic: Diagnostic.Create(
                    descriptor: Descriptors.SourceGenerationFailedDiagnostic,
                    location: Location.Create(
                        syntaxTree: context.Compilation.SyntaxTrees.First(),
                        textSpan: TextSpan.FromBounds(0, 100)),
                    messageArgs: ex.ToString()));
        }
    }

    private static bool IsDiagnosticError(GeneratorOptions options, GeneratorExecutionContext context, AttributeSyntax attribute)
    {
        if (options.TypeName is null)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Descriptors.TypeNameRequiredDiagnostic,
                    attribute.GetLocation()));

            return true;
        }

        if (options.Implementation is null)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Descriptors.PathFromWindowRequiredDiagnostic,
                    attribute.GetLocation()));

            return true;
        }

        if (options.SupportsGenerics &&
            !context.Compilation.ReferencedAssemblyNames.Any(
                ai => ai.Name.Equals("Blazor.Serialization", StringComparison.OrdinalIgnoreCase)))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Descriptors.MissingBlazorSerializationPackageReferenceDiagnostic,
                    attribute.GetLocation()));

            return true;
        }

        return false;
    }
}