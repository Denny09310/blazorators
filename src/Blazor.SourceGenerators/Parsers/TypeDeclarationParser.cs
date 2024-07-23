// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators.Parsers;

internal sealed partial class TypeDeclarationParser
{
    private static readonly Lazy<TypeDeclarationParser> _parser =
        new(valueFactory: () => new TypeDeclarationParser(TypeDeclarationReader.Default));

    private readonly TypeMapperBuilder _mapper;
    private readonly TypeDeclarationReader _reader;

    internal TypeDeclarationParser(TypeDeclarationReader reader)
    {
        _reader = reader;
        _mapper = new TypeMapperBuilder().WithReader(_reader);
    }

    internal static TypeDeclarationParser Default => _parser.Value;

    public ParserResult<CSharpTopLevelObject> ParseTargetType(string typeName)
    {
        ParserResult<CSharpTopLevelObject> result = new(ParserResultStatus.Unknown);

        var descriptor = _mapper.Build(typeName);

        if (descriptor == default) return result with
        {
            Status = ParserResultStatus.TargetTypeNotFound
        };

        try
        {
            result = result with
            {
                Status = ParserResultStatus.SuccessfullyParsed,
                Value = ToTopLevelObject(descriptor)
            };
        }
        catch (Exception ex)
        {
            result = result with
            {
                Status = ParserResultStatus.ErrorParsing,
                Error = ex.Message
            };
        }

        return result;
    }
}