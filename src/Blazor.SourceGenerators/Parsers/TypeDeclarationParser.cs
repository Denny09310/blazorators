// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators.Parsers;

internal sealed partial class TypeDeclarationParser
{
    private static readonly Lazy<TypeDeclarationParser> _parser =
        new(valueFactory: () => new TypeDeclarationParser(TypeDeclarationReader.Default));

    private readonly DependencyMapBuilder _mapper;
    private readonly TypeDeclarationReader _reader;

    internal TypeDeclarationParser(TypeDeclarationReader reader)
    {
        _reader = reader;
        _mapper = new DependencyMapBuilder().WithReader(_reader);
    }

    internal static TypeDeclarationParser Default => _parser.Value;

    public ParserResult<CSharpTopLevelObject> ParseTargetType(string typeName)
    {
        ParserResult<CSharpTopLevelObject> result = new(ParserResultStatus.Unknown);

        _mapper.Build(typeName);

        if (_mapper.Root == default) return result with
        {
            Status = ParserResultStatus.TargetTypeNotFound
        };

        try
        {
            result = result with
            {
                Status = ParserResultStatus.SuccessfullyParsed,
                Value = ToTopLevelObject(_mapper.Root)
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