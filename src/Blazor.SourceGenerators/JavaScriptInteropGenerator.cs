// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators;

[Generator]
internal sealed partial class JavaScriptInteropGenerator : IIncrementalGenerator
{
    private readonly HashSet<(string FileName, string SourceCode)> _sources =
    [
        (nameof(RecordCompat).ToGeneratedFileName(), RecordCompat),
        (nameof(BlazorHostingModel).ToGeneratedFileName(), BlazorHostingModel),
        (nameof(JSAutoInteropAttribute).ToGeneratedFileName(), JSAutoInteropAttribute),
        (nameof(JSAutoGenericInteropAttribute).ToGeneratedFileName(), JSAutoGenericInteropAttribute),
    ];

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput((ctx) =>
        {
            foreach (var (fileName, sourceCode) in _sources)
            {
                ctx.AddSource(fileName, SourceText.From(sourceCode, Encoding.UTF8));
            }
        });

        var syntaxProvider = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(m => m is not null)
            .WithComparer(new JavaScriptIncrementalValuesComparer());

        context.RegisterSourceOutput(
            syntaxProvider,
            static (spc, source) => Execute(source, spc));
    }

    private static void Execute(InterfaceDeclarationDetails source, SourceProductionContext context)
    {
        var cancellationToken = context.CancellationToken;
        var (options, interfaceDeclaration, _, syntaxTree, _, containingNamespace) = source;

        try
        {
            foreach (var parser in options.Parsers)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = parser.ParseTargetType(options.TypeName!);
                if (result is not { Status: ParserResultStatus.SuccessfullyParsed, Value: { } })
                {
                    continue;
                }

                var namespaceString = (containingNamespace, interfaceDeclaration.Parent) switch
                {
                    (string { Length: > 0 }, _) => containingNamespace,
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
                        syntaxTree,
                        textSpan: TextSpan.FromBounds(0, 100)),
                    messageArgs: ex.ToString()));
        }
    }

    private static InterfaceDeclarationDetails GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        var interfaceDeclaration = (InterfaceDeclarationSyntax)context.Node;

        var isPartial = interfaceDeclaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PartialKeyword));
        if (!isPartial) return null!;

        var typeSymbol = context.SemanticModel.GetDeclaredSymbol(interfaceDeclaration);
        if (typeSymbol is not ITypeSymbol symbol || symbol.IsStatic)
        {
            return null!;
        }

        var attribute = interfaceDeclaration.AttributeLists
            .SelectMany(attributeList => attributeList.Attributes)
            .FirstOrDefault(attribute => attribute.Name.ToString() is "JSAutoInterop" or "JSAutoGenericInterop");

        if (attribute == null) return null!;

        return new(Options: attribute.GetGeneratorOptions(attribute.Name.ToString() is "JSAutoGenericInterop"),
                   InterfaceDeclaration: interfaceDeclaration,
                   InteropAttribute: attribute,
                   SyntaxTree: context.Node.SyntaxTree,
                   InterfaceName: typeSymbol.Name,
                   ContainingNamespace: typeSymbol.ContainingNamespace.ToDisplayString());
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode syntax)
    {
        return syntax is InterfaceDeclarationSyntax interfaceDeclaration
            && interfaceDeclaration.AttributeLists.Any(attributeList => attributeList.Attributes
                .Any(attribute => attribute.Name.ToString() is "JSAutoInterop" or "JSAutoGenericInterop"));
    }
}