﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;
using static Blazor.SourceGenerators.TypeScript.Compiler.Scanner;
using SyntaxKind = Blazor.SourceGenerators.TypeScript.Types.TypeScriptSyntaxKind;

#nullable disable
namespace Blazor.SourceGenerators.TypeScript.Compiler;

internal class JsDocParser
{
    public Parser Parser { get; }

    public JsDocParser(Parser parser) => Parser = parser;

    private Scanner Scanner => Parser.Scanner;
    private string SourceText => Parser.SourceText;
    private SyntaxKind CurrentToken { get => Parser.CurrentToken; set => Parser.CurrentToken = value; }
    private bool ParseErrorBeforeNextFinishedNode { get => Parser.ParseErrorBeforeNextFinishedNode; set => Parser.ParseErrorBeforeNextFinishedNode = value; }
    private List<TypeScriptDiagnostic> ParseDiagnostics { get => Parser.ParseDiagnostics; set => Parser.ParseDiagnostics = value; }

    private void ClearState() => Parser.ClearState();
    private void FixupParentReferences(INode rootNode) => Parser.FixupParentReferences(rootNode);
    private void ParseErrorAtCurrentToken(DiagnosticMessage? message, object arg0 = null) => Parser.ParseErrorAtCurrentToken(message, arg0);
    private void ParseErrorAtPosition(int start, int length, DiagnosticMessage? message, object arg0 = null) => Parser.ParseErrorAtPosition(start, length, message, arg0);
    private SyntaxKind Token => Parser.CurrentToken;
    private SyntaxKind NextToken => Parser.NextToken();
    private T TryParse<T>(Func<T> callback) => Parser.TryParse(callback);
    private void ParseExpected(SyntaxKind kind, DiagnosticMessage? diagnosticMessage = null, bool shouldAdvance = true) => Parser.ParseExpected(kind, diagnosticMessage, shouldAdvance);
    private bool ParseOptional(SyntaxKind t) => Parser.ParseOptional(t);
    private Node ParseOptionalToken<T>(SyntaxKind t) where T : Node, new() => Parser.ParseOptionalToken<T>(t);
    private T ParseTokenNode<T>() where T : Node, new() => Parser.ParseTokenNode<T>(Token);
    private NodeArray<T> CreateList<T>(T[] elements = null, int? pos = null) => Parser.CreateList(elements, pos);
    private T FinishNode<T>(T node, int? end = null) where T : Node => Parser.FinishNode(node, end);
    private Identifier ParseIdentifierName() => Parser.ParseIdentifierName();
    private NodeArray<T> ParseDelimitedList<T>(ParsingContext kind, Func<T> parseElement, bool? considerSemicolonAsDelimiter = null) where T : INode => Parser.ParseDelimitedList(kind, parseElement, considerSemicolonAsDelimiter);
    private TypeLiteralNode ParseTypeLiteral() => Parser.ParseTypeLiteral();
    private void ParseExpression() => Parser.ParseExpression();

    public bool IsJsDocType() => Token switch
    {
        SyntaxKind.AsteriskToken or SyntaxKind.QuestionToken or SyntaxKind.OpenParenToken or SyntaxKind.OpenBracketToken or SyntaxKind.ExclamationToken or SyntaxKind.OpenBraceToken or SyntaxKind.FunctionKeyword or SyntaxKind.DotDotDotToken or SyntaxKind.NewKeyword or SyntaxKind.ThisKeyword => true,
        _ => TokenIsIdentifierOrKeyword(Token),
    };

    public static (JsDocTypeExpression res, List<TypeScriptDiagnostic> diagnostics) ParseJsDocTypeExpressionForTests(string content, int? start, int? length)
    {
        var dp = new JsDocParser(new Parser());
        var languageVersion = ScriptTarget.Latest;
        dp.Parser.InitializeState(content, languageVersion, ScriptKind.Js);

        /* sourceFile */
        dp.Parser.CreateSourceFile("file.js", languageVersion, ScriptKind.Js);

        dp.Parser.Scanner.SetText(content, start, length);

        /* currentToken */
        dp.Parser.Scanner.Scan();
        var jsDocTypeExpression = dp.ParseJsDocTypeExpression();
        var diagnostics = dp.Parser.ParseDiagnostics;

        dp.Parser.ClearState();

        return (jsDocTypeExpression, diagnostics);
    }

    public JsDocTypeExpression ParseJsDocTypeExpression()
    {
        var result = new JsDocTypeExpression();

        ParseExpected(SyntaxKind.OpenBraceToken);

        result.Type = ParseJsDocTopLevelType();

        ParseExpected(SyntaxKind.CloseBraceToken);

        FixupParentReferences(result);

        return FinishNode(result);
    }

    public IJsDocType ParseJsDocTopLevelType()
    {
        var type = ParseJsDocType();
        if (Token == SyntaxKind.BarToken)
        {
            var unionType = new JsDocUnionType { Types = ParseJsDocTypeList(type) };

            type = FinishNode(unionType);
        }
        if (Token == SyntaxKind.EqualsToken)
        {
            var optionalType = new JsDocOptionalType();

            _ = NextToken;

            optionalType.Type = type;

            type = FinishNode(optionalType);
        }

        return type;
    }

    public IJsDocType ParseJsDocType()
    {
        var type = ParseBasicTypeExpression();
        while (true)
        {
            if (Token == SyntaxKind.OpenBracketToken)
            {
                var arrayType = new JsDocArrayType { ElementType = type };
                _ = NextToken;
                ParseExpected(SyntaxKind.CloseBracketToken);

                type = FinishNode(arrayType);
            }
            else if (Token == SyntaxKind.QuestionToken)
            {
                var nullableType = new JsDocNullableType { Type = type };

                _ = NextToken;

                type = FinishNode(nullableType);
            }
            else if (Token == SyntaxKind.ExclamationToken)
            {
                var nonNullableType = new JsDocNonNullableType { Type = type };

                _ = NextToken;

                type = FinishNode(nonNullableType);
            }
            else
            {
                break;
            }
        }

        return type;
    }

    public IJsDocType ParseBasicTypeExpression() => Token switch
    {
        SyntaxKind.AsteriskToken => ParseJsDocAllType(),
        SyntaxKind.QuestionToken => ParseJsDocUnknownOrNullableType(),
        SyntaxKind.OpenParenToken => ParseJsDocUnionType(),
        SyntaxKind.OpenBracketToken => ParseJsDocTupleType(),
        SyntaxKind.ExclamationToken => ParseJsDocNonNullableType(),
        SyntaxKind.OpenBraceToken => ParseJsDocRecordType(),
        SyntaxKind.FunctionKeyword => ParseJsDocFunctionType(),
        SyntaxKind.DotDotDotToken => ParseJsDocVariadicType(),
        SyntaxKind.NewKeyword => ParseJsDocConstructorType(),
        SyntaxKind.ThisKeyword => ParseJsDocThisType(),
        SyntaxKind.AnyKeyword or SyntaxKind.StringKeyword or SyntaxKind.NumberKeyword or SyntaxKind.BooleanKeyword or SyntaxKind.SymbolKeyword or SyntaxKind.VoidKeyword or SyntaxKind.NullKeyword or SyntaxKind.UndefinedKeyword or SyntaxKind.NeverKeyword or SyntaxKind.ObjectKeyword => ParseTokenNode<JsDocType>(),
        SyntaxKind.StringLiteral or SyntaxKind.NumericLiteral or SyntaxKind.TrueKeyword or SyntaxKind.FalseKeyword => ParseJsDocLiteralType(),
        _ => ParseJsDocTypeReference(),
    };

    public JsDocThisType ParseJsDocThisType()
    {
        var result = new JsDocThisType();

        _ = NextToken;

        ParseExpected(SyntaxKind.ColonToken);

        result.Type = ParseJsDocType();

        return FinishNode(result);
    }

    public JsDocConstructorType ParseJsDocConstructorType()
    {
        var result = new JsDocConstructorType();

        _ = NextToken;

        ParseExpected(SyntaxKind.ColonToken);

        result.Type = ParseJsDocType();

        return FinishNode(result);
    }

    public JsDocVariadicType ParseJsDocVariadicType()
    {
        var result = new JsDocVariadicType();

        _ = NextToken;

        result.Type = ParseJsDocType();

        return FinishNode(result);
    }

    public JsDocFunctionType ParseJsDocFunctionType()
    {
        var result = new JsDocFunctionType();

        _ = NextToken;

        ParseExpected(SyntaxKind.OpenParenToken);

        result.Parameters = Parser.ParseDelimitedList(ParsingContext.JSDocFunctionParameters, ParseJsDocParameter);

        CheckForTrailingComma(result.Parameters);

        ParseExpected(SyntaxKind.CloseParenToken);
        if (Token == SyntaxKind.ColonToken)
        {
            _ = NextToken;

            result.Type = ParseJsDocType();
        }

        return FinishNode(result);
    }

    public ParameterDeclaration ParseJsDocParameter()
    {
        var parameter = new ParameterDeclaration { Type = ParseJsDocType() };

        if (ParseOptional(SyntaxKind.EqualsToken))
        {
            parameter.QuestionToken = new QuestionToken { };
        }

        return FinishNode(parameter);
    }

    public JsDocTypeReference ParseJsDocTypeReference()
    {
        var result = new JsDocTypeReference { Name = Parser.ParseSimplePropertyName() as Identifier };

        if (Token == SyntaxKind.LessThanToken)
        {
            result.TypeArguments = ParseTypeArguments();
        }
        else
        {
            while (ParseOptional(SyntaxKind.DotToken))
            {
                if (Token == SyntaxKind.LessThanToken)
                {
                    result.TypeArguments = ParseTypeArguments();
                    break;
                }
                else
                {
                    result.Name = ParseQualifiedName(result.Name);
                }
            }
        }

        return FinishNode(result);
    }

    public NodeArray<IJsDocType> ParseTypeArguments()
    {
        _ = NextToken;
        var typeArguments = ParseDelimitedList(ParsingContext.JSDocTypeArguments, ParseJsDocType);

        CheckForTrailingComma(typeArguments);

        CheckForEmptyTypeArgumentList(typeArguments);

        ParseExpected(SyntaxKind.GreaterThanToken);

        return typeArguments;
    }

    public void CheckForEmptyTypeArgumentList<T>(NodeArray<T> typeArguments)
    {
        if (ParseDiagnostics.Count == 0 && typeArguments != null && !typeArguments.Any())
        {
            var start = (typeArguments.Pos ?? 0) - "<".Length;
            var end = SkipTriviaM(SourceText, (int)typeArguments.End) + ">".Length;

            ParseErrorAtPosition(start, end - start, Diagnostics.Type_argument_list_cannot_be_empty);
        }
    }

    public QualifiedName ParseQualifiedName(IEntityName left)
    {
        var result = new QualifiedName
        {
            Left = left,
            Right = ParseIdentifierName()
        };

        return FinishNode(result);
    }

    public JsDocRecordType ParseJsDocRecordType()
    {
        var result = new JsDocRecordType { Literal = ParseTypeLiteral() };

        return FinishNode(result);
    }

    public JsDocNonNullableType ParseJsDocNonNullableType()
    {
        var result = new JsDocNonNullableType();

        _ = NextToken;

        result.Type = ParseJsDocType();

        return FinishNode(result);
    }

    public JsDocTupleType ParseJsDocTupleType()
    {
        var result = new JsDocTupleType();

        _ = NextToken;

        result.Types = ParseDelimitedList(ParsingContext.JSDocTupleTypes, ParseJsDocType);

        CheckForTrailingComma(result.Types);

        ParseExpected(SyntaxKind.CloseBracketToken);

        return FinishNode(result);
    }

    public void CheckForTrailingComma<T>(NodeArray<T> list)
    {
        if (Parser.ParseDiagnostics.Count == 0 && list.HasTrailingComma)
        {
            var start = list.End - ",".Length;

            Parser.ParseErrorAtPosition((int)start, ",".Length, Diagnostics.Trailing_comma_not_allowed);
        }
    }

    public JsDocUnionType ParseJsDocUnionType()
    {
        var result = new JsDocUnionType();

        _ = NextToken;

        result.Types = ParseJsDocTypeList(ParseJsDocType());

        ParseExpected(SyntaxKind.CloseParenToken);

        return FinishNode(result);
    }

    public NodeArray<IJsDocType> ParseJsDocTypeList(IJsDocType firstType)
    {

        var types = Parser.CreateList<IJsDocType>();
        types.Add(firstType);
        types.Pos = firstType.Pos;
        while (ParseOptional(SyntaxKind.BarToken))
        {

            types.Add(ParseJsDocType());
        }

        types.End = Scanner.StartPos;

        return types;
    }

    public JsDocAllType ParseJsDocAllType()
    {
        var result = new JsDocAllType();

        _ = NextToken;

        return FinishNode(result);
    }

    public JsDocLiteralType ParseJsDocLiteralType()
    {
        var result = new JsDocLiteralType { Literal = Parser.ParseLiteralTypeNode() };

        return FinishNode(result);
    }

    public JsDocType ParseJsDocUnknownOrNullableType()
    {
        _ = NextToken;
        if (Token is SyntaxKind.CommaToken or SyntaxKind.CloseBraceToken or SyntaxKind.CloseParenToken or SyntaxKind.GreaterThanToken or SyntaxKind.EqualsToken or SyntaxKind.BarToken)
        {
            var result = new JsDocUnknownType();

            return FinishNode(result);
        }
        else
        {
            var result = new JsDocNullableType { Type = ParseJsDocType() };

            return FinishNode(result);
        }
    }

    public Tuple<JsDoc, List<TypeScriptDiagnostic>> ParseIsolatedJsDocComment(string content, int start, int length)
    {
        Parser.InitializeState(content, ScriptTarget.Latest, ScriptKind.Js);
        Parser.SourceFile = new() { LanguageVariant = LanguageVariant.Standard, Text = content };
        var jsDoc = ParseJsDocCommentWorker(start, length);
        var diagnostics = ParseDiagnostics;

        ClearState();

        return jsDoc != null ? Tuple.Create(jsDoc, diagnostics) : null;
    }

    public JsDoc ParseJsDocComment(INode parent, int? start, int? length)
    {
        var saveToken = CurrentToken;
        var saveParseDiagnosticsLength = Parser.ParseDiagnostics.Count;
        var saveParseErrorBeforeNextFinishedNode = ParseErrorBeforeNextFinishedNode;
        var comment = ParseJsDocCommentWorker(start, length);
        if (comment != null)
        {
            comment.Parent = parent;
        }

        CurrentToken = saveToken;
        ParseDiagnostics = ParseDiagnostics.Take(saveParseDiagnosticsLength).ToList();
        ParseErrorBeforeNextFinishedNode = saveParseErrorBeforeNextFinishedNode;

        return comment;
    }

    public JsDoc ParseJsDocCommentWorker(int? start = null, int? length = null)
    {
        var content = Parser.SourceText;

        var strt = start.GetValueOrDefault();
        var end = length == null ? content.Length : strt + length;

        length = end - strt;

        Debug.Assert(strt >= 0);
        Debug.Assert(strt <= end);

        Debug.Assert(end <= content.Length);
        NodeArray<IJsDocTag> tags = [];
        List<string> comments = [];
        JsDoc result = null;
        if (!IsJsDocStart(content, strt))
        {
            return result;
        }

        Scanner.ScanRange<JsDoc>(strt + 3, (length ?? 0) - 5, () =>
        {
            var advanceToken = true;
            var state = JSDocState.SawAsterisk;
            int? margin = null;
            var indent = start - Math.Max(content.LastIndexOf('\n', (int)start), 0) + 4;
            NextJsDocToken();
            while (Token == SyntaxKind.WhitespaceTrivia)
            {
                NextJsDocToken();
            }
            if (Token == SyntaxKind.NewLineTrivia)
            {
                state = JSDocState.BeginningOfLine;
                indent = 0;
                NextJsDocToken();
            }
            while (Token != SyntaxKind.EndOfFileToken)
            {
                switch (Token)
                {
                    case SyntaxKind.AtToken:
                        if (state is JSDocState.BeginningOfLine or JSDocState.SawAsterisk)
                        {
                            RemoveTrailingNewlines(comments);
                            ParseTag((int)indent);
                            state = JSDocState.BeginningOfLine;
                            advanceToken = false;
                            margin = null;

                            indent++;
                        }
                        else
                        {
                            PushComment(Scanner.TokenText);
                        }

                        break;
                    case SyntaxKind.NewLineTrivia:
                        comments.Add(Scanner.TokenText);
                        state = JSDocState.BeginningOfLine;

                        indent = 0;

                        break;
                    case SyntaxKind.AsteriskToken:
                        var asterisk = Scanner.TokenText;
                        if (state is JSDocState.SawAsterisk or JSDocState.SavingComments)
                        {
                            state = JSDocState.SavingComments;

                            PushComment(asterisk);
                        }
                        else
                        {

                            state = JSDocState.SawAsterisk;

                            indent += asterisk.Length;
                        }

                        break;
                    case SyntaxKind.Identifier:

                        PushComment(Scanner.TokenText);

                        state = JSDocState.SavingComments;

                        break;
                    case SyntaxKind.WhitespaceTrivia:
                        var whitespace = Scanner.TokenText;
#pragma warning disable S2583 // Conditionally executed code should be reachable

                        if (state == JSDocState.SavingComments)
                        {

                            comments.Add(whitespace);
                        }
                        else if (margin != null && (indent ?? 0) + whitespace.Length > margin)
                        {

                            comments.Add(whitespace.Slice((int)margin - (indent ?? 0) - 1));
                        }

#pragma warning restore S2583 // Conditionally executed code should be reachable

                        indent += whitespace.Length;

                        break;
                    case SyntaxKind.EndOfFileToken:

                        break;
                    default:
                        state = JSDocState.SavingComments;

                        PushComment(Scanner.TokenText);

                        break;
                }
                if (advanceToken)
                {
                    NextJsDocToken();
                }
                else
                {
                    advanceToken = true;
                }
            }

            RemoveLeadingNewlines(comments);

            RemoveTrailingNewlines(comments);

            result = CreateJsDocComment();
            return result;
            void PushComment(string text)
            {
                margin ??= indent;

                comments.Add(text);

                indent += text.Length;
            }
        });

        return result;

        void RemoveLeadingNewlines(List<string> comments3)
        {
            while (comments3.Count != 0 && (comments3[0] == "\n" || comments3[0] == "\r"))
            {
                comments3 = comments3.Skip(1).ToList();
            }
        }

        void RemoveTrailingNewlines(List<string> comments2)
        {
            while (comments2.Count != 0 && (comments2[comments2.Count - 1] == "\n" || comments2[comments2.Count - 1] == "\r"))
            {
                comments2.Pop();
            }
        }

        bool IsJsDocStart(string content2, int start2) => content2[start2] == '/' &&
                content2[start2 + 1] == '*' &&
                content2[start2 + 2] == '*' &&
                content2[start2 + 3] != '*';

        JsDoc CreateJsDocComment()
        {
            var result2 = new JsDoc
            {
                Tags = tags,
                Comment = comments.Count != 0 ? string.Join("", comments) : null
            };

            return FinishNode(result2, end);
        }

        void SkipWhitespace()
        {
            while (Token is SyntaxKind.WhitespaceTrivia or SyntaxKind.NewLineTrivia)
            {
                NextJsDocToken();
            }
        }

        void ParseTag(int indent)
        {

            Debug.Assert(Token == SyntaxKind.AtToken);
            var atToken = new AtToken { End = Scanner.TextPos };

            NextJsDocToken();
            var tagName = ParseJsDocIdentifierName();

            SkipWhitespace();
            if (tagName == null)
            {

                return;
            }
            IJsDocTag tag = null;
#pragma warning disable S2583 // Conditionally executed code should be reachable

            if (tagName != null)
            {
                tag = tagName.Text switch
                {
                    "augments" => ParseAugmentsTag(atToken, tagName),
                    "param" => ParseParamTag(atToken, tagName),
                    "return" or "returns" => ParseReturnTag(atToken, tagName),
                    "template" => ParseTemplateTag(atToken, tagName),
                    "type" => ParseTypeTag(atToken, tagName),
                    "typedef" => ParseTypedefTag(atToken, tagName),
                    _ => ParseUnknownTag(atToken, tagName),
                };
            }
            else
            {

                tag = ParseUnknownTag(atToken, tagName);
            }

#pragma warning restore S2583 // Conditionally executed code should be reachable
            if (tag == null)
            {

                return;
            }

            AddTag(tag, ParseTagComments(indent + (tag.End ?? 0) - (tag.Pos ?? 0)));
        }

        List<string> ParseTagComments(int indent)
        {
            List<string> comments2 = [];
            var state = JSDocState.SawAsterisk;
            int? margin = null;
            while (Token != SyntaxKind.AtToken && Token != SyntaxKind.EndOfFileToken)
            {
                switch (Token)
                {
                    case SyntaxKind.NewLineTrivia:
                        if (state >= JSDocState.SawAsterisk)
                        {

                            state = JSDocState.BeginningOfLine;

                            comments2.Add(Scanner.TokenText);
                        }

                        indent = 0;

                        break;
                    case SyntaxKind.AtToken:

                        break;
                    case SyntaxKind.WhitespaceTrivia:
                        if (state == JSDocState.SavingComments)
                        {

                            PushComment(Scanner.TokenText);
                        }
                        else
                        {
                            var whitespace = Scanner.TokenText;
#pragma warning disable S2583 // Conditionally executed code should be reachable

                            if (margin != null && indent + whitespace.Length > margin)
                            {

                                comments2.Add(whitespace.Slice((int)margin - indent - 1));
                            }

#pragma warning restore S2583 // Conditionally executed code should be reachable

                            indent += whitespace.Length;
                        }

                        break;
                    case SyntaxKind.AsteriskToken:
                        if (state == JSDocState.BeginningOfLine)
                        {

                            state = JSDocState.SawAsterisk;

                            indent += Scanner.TokenText.Length;

                            break;
                        }
#pragma warning disable S907 // "goto" statement should not be used
                        goto caseLabel5;
#pragma warning restore S907 // "goto" statement should not be used
                    default:

caseLabel5: state = JSDocState.SavingComments;
                        PushComment(Scanner.TokenText);

                        break;
                }
                if (Token == SyntaxKind.AtToken)
                {

                    break;
                }

                NextJsDocToken();
            }

            RemoveLeadingNewlines(comments2);

            RemoveTrailingNewlines(comments2);

            return comments2;
            void PushComment(string text)
            {
                margin ??= indent;

                comments2.Add(text);

                indent += text.Length;
            }
        }

        JsDocTag ParseUnknownTag(AtToken atToken, Identifier tagName)
        {
            var result2 = new JsDocTag
            {
                AtToken = atToken,
                TagName = tagName
            };

            return FinishNode(result2);
        }

        void AddTag(IJsDocTag tag, List<string> comments2)
        {

            tag.Comment = string.Join("", comments2);
            if (tags == null)
            {
                tags = Parser.CreateList<IJsDocTag>();
                tags.Pos = tag.Pos;

            }
            else
            {
                tags.Add(tag);
            }

            tags.End = tag.End;
        }

        JsDocTypeExpression TryParseTypeExpression() => TryParse(
            () =>
            {
                SkipWhitespace();
                return Token != SyntaxKind.OpenBraceToken ? null : ParseJsDocTypeExpression();
            });

        JsDocParameterTag ParseParamTag(AtToken atToken, Identifier tagName)
        {
            var typeExpression = TryParseTypeExpression();

            SkipWhitespace();
            Identifier name = null;
            var isBracketed = false;
            if ((OpenBracketToken)ParseOptionalToken<OpenBracketToken>(SyntaxKind.OpenBracketToken) != null)
            {

                name = ParseJsDocIdentifierName();

                SkipWhitespace();

                isBracketed = true;
                if ((EqualsToken)ParseOptionalToken<EqualsToken>(SyntaxKind.EqualsToken) != null)
                {

                    ParseExpression();
                }

                ParseExpected(SyntaxKind.CloseBracketToken);
            }
            else if (TokenIsIdentifierOrKeyword(Token))
            {

                name = ParseJsDocIdentifierName();
            }
            if (name == null)
            {

                ParseErrorAtPosition(Scanner.StartPos, 0, Diagnostics.Identifier_expected);

                return null;
            }
            Identifier preName = null;
            Identifier postName = null;
            if (typeExpression != null)
            {
                postName = name;
            }
            else
            {
                preName = name;
            }
            typeExpression ??= TryParseTypeExpression();
            var result4 = new JsDocParameterTag
            {
                AtToken = atToken,
                TagName = tagName,
                PreParameterName = preName,
                TypeExpression = typeExpression,
                PostParameterName = postName,
                ParameterName = postName ?? preName,
                IsBracketed = isBracketed
            };


            return FinishNode(result4);
        }

        JsDocReturnTag ParseReturnTag(AtToken atToken, Identifier tagName)
        {
            if (tags.Exists(t => t.Kind == SyntaxKind.JsDocReturnTag))
            {

                ParseErrorAtPosition(tagName.Pos ?? 0, Scanner.TokenPos - (tagName.Pos ?? 0), Diagnostics._0_tag_already_specified, tagName.Text);
            }
            var result5 = new JsDocReturnTag
            {
                AtToken = atToken,
                TagName = tagName,
                TypeExpression = TryParseTypeExpression()
            };

            return FinishNode(result5);
        }

        JsDocTypeTag ParseTypeTag(AtToken atToken, Identifier tagName)
        {
            if (tags.Exists(t => t.Kind == SyntaxKind.JsDocTypeTag))
            {

                ParseErrorAtPosition(tagName.Pos ?? 0, Scanner.TokenPos - (tagName.Pos ?? 0), Diagnostics._0_tag_already_specified, tagName.Text);
            }
            var result6 = new JsDocTypeTag
            {
                AtToken = atToken,
                TagName = tagName,
                TypeExpression = TryParseTypeExpression()
            };

            return FinishNode(result6);
        }

        JsDocPropertyTag ParsePropertyTag(AtToken atToken, Identifier tagName)
        {
            var typeExpression = TryParseTypeExpression();

            SkipWhitespace();
            var name = ParseJsDocIdentifierName();

            SkipWhitespace();
            if (name == null)
            {

                ParseErrorAtPosition(Scanner.StartPos, 0, Diagnostics.Identifier_expected);

                return null;
            }
            var result7 = new JsDocPropertyTag
            {
                AtToken = atToken,
                TagName = tagName,
                Name = name,
                TypeExpression = typeExpression
            };

            return FinishNode(result7);
        }

        JsDocAugmentsTag ParseAugmentsTag(AtToken atToken, Identifier tagName)
        {
            var typeExpression = TryParseTypeExpression();
            var result8 = new JsDocAugmentsTag
            {
                AtToken = atToken,
                TagName = tagName,
                TypeExpression = typeExpression
            };

            return FinishNode(result8);
        }

        IJsDocTag ParseTypedefTag(AtToken atToken, Identifier tagName)
        {
            var typeExpression = TryParseTypeExpression();

            SkipWhitespace();
            var typedefTag = new JsDocTypedefTag
            {
                AtToken = atToken,
                TagName = tagName,
                FullName = ParseJsDocTypeNameWithNamespace(0)
            };

            if (typedefTag.FullName != null)
            {
                var rightNode = typedefTag.FullName;
                while (true)
                {
                    if (rightNode?.Kind == SyntaxKind.Identifier || (rightNode as JsDocNamespaceDeclaration)?.Body == null)
                    {

                        typedefTag.Name = rightNode?.Kind == SyntaxKind.Identifier ? rightNode : (rightNode as JsDocTypedefTag)?.Name;

                        break;
                    }

                    rightNode = (rightNode as JsDocNamespaceDeclaration)?.Body;
                }
            }

            typedefTag.TypeExpression = typeExpression;

            SkipWhitespace();
            if (typeExpression != null)
            {
                if (typeExpression.Type.Kind == SyntaxKind.JsDocTypeReference)
                {
                    var jsDocTypeReference = (JsDocTypeReference)typeExpression.Type;
                    if (jsDocTypeReference.Name.Kind == SyntaxKind.Identifier)
                    {
                        var name = jsDocTypeReference.Name as Identifier;
                        if (name?.Text == "Object")
                        {

                            typedefTag.JsDocTypeLiteral = ScanChildTags();
                        }
                    }
                }
                typedefTag.JsDocTypeLiteral ??= (JsDocTypeLiteral)typeExpression.Type;
            }
            else
            {

                typedefTag.JsDocTypeLiteral = ScanChildTags();
            }

            return FinishNode(typedefTag);
        }

        JsDocTypeLiteral ScanChildTags()
        {
            var jsDocTypeLiteral = new JsDocTypeLiteral();
            var resumePos = Scanner.StartPos;
            var canParseTag = true;
            var seenAsterisk = false;
            var parentTagTerminated = false;
            while (Token != SyntaxKind.EndOfFileToken && !parentTagTerminated)
            {
                NextJsDocToken();
                switch (Token)
                {
                    case SyntaxKind.AtToken:
                        if (canParseTag)
                        {

                            parentTagTerminated = !TryParseChildTag(jsDocTypeLiteral);
                            if (!parentTagTerminated)
                            {

                                resumePos = Scanner.StartPos;
                            }
                        }

                        seenAsterisk = false;

                        break;
                    case SyntaxKind.NewLineTrivia:

                        resumePos = Scanner.StartPos - 1;

                        canParseTag = true;

                        seenAsterisk = false;

                        break;
                    case SyntaxKind.AsteriskToken:
                        if (seenAsterisk)
                        {

                            canParseTag = false;
                        }

                        seenAsterisk = true;

                        break;
                    case SyntaxKind.Identifier:

                        canParseTag = false;
#pragma warning disable S907 // "goto" statement should not be used
                        goto caseLabel5;
#pragma warning restore S907 // "goto" statement should not be used
                    case SyntaxKind.EndOfFileToken:
caseLabel5: break;
                }
            }

            Scanner.SetTextPos(resumePos);

            return FinishNode(jsDocTypeLiteral);
        }

        INode ParseJsDocTypeNameWithNamespace(NodeFlags flags)
        {
            var typeNameOrNamespaceName = ParseJsDocIdentifierName();
            if (typeNameOrNamespaceName != null && ParseOptional(SyntaxKind.DotToken))
            {
                var jsDocNamespaceNode = new JsDocNamespaceDeclaration();

                jsDocNamespaceNode.Flags |= flags;
                jsDocNamespaceNode.Name = typeNameOrNamespaceName;
                jsDocNamespaceNode.Body = ParseJsDocTypeNameWithNamespace(NodeFlags.NestedNamespace);

                return jsDocNamespaceNode;
            }
            if (typeNameOrNamespaceName != null && (flags & NodeFlags.NestedNamespace) != 0)
            {

                typeNameOrNamespaceName.IsInJsDocNamespace = true;
            }

            return typeNameOrNamespaceName;
        }

        bool TryParseChildTag(JsDocTypeLiteral parentTag)
        {

            Debug.Assert(Token == SyntaxKind.AtToken);
            var atToken = new AtToken { End = Scanner.TextPos };

            NextJsDocToken();
            var tagName = ParseJsDocIdentifierName();

            SkipWhitespace();
            if (tagName == null)
            {

                return false;
            }
            switch (tagName.Text)
            {
                case "type":
                    if (parentTag.JsDocTypeTag != null)
                    {

                        return false;
                    }

                    parentTag.JsDocTypeTag = ParseTypeTag(atToken, tagName);

                    return true;
                case "prop":
                case "property":
                    var propertyTag = ParsePropertyTag(atToken, tagName);
                    if (propertyTag != null)
                    {
                        parentTag.JsDocPropertyTags ??= [];

                        parentTag.JsDocPropertyTags.Add(propertyTag);

                        return true;
                    }

                    return false;
            }

            return false;
        }

        JsDocTemplateTag ParseTemplateTag(AtToken atToken, Identifier tagName)
        {
            if (tags.Exists(t => t.Kind == SyntaxKind.JsDocTemplateTag))
            {

                ParseErrorAtPosition(tagName.Pos ?? 0, Scanner.TokenPos - (tagName.Pos ?? 0), Diagnostics._0_tag_already_specified, tagName.Text);
            }
            var typeParameters = CreateList<TypeParameterDeclaration>();
            while (true)
            {
                var name = ParseJsDocIdentifierName();

                SkipWhitespace();
                if (name == null)
                {

                    ParseErrorAtPosition(Scanner.StartPos, 0, Diagnostics.Identifier_expected);

                    return null;
                }
                var typeParameter = new TypeParameterDeclaration { Name = name };

                FinishNode(typeParameter);

                typeParameters.Add(typeParameter);
                if (Token == SyntaxKind.CommaToken)
                {
                    NextJsDocToken();
                    SkipWhitespace();
                }
                else
                {

                    break;
                }
            }
            var result3 = new JsDocTemplateTag
            {
                AtToken = atToken,
                TagName = tagName,
                TypeParameters = typeParameters
            };

            FinishNode(result3);

            typeParameters.End = result3.End;

            return result3;
        }

        void NextJsDocToken()
        {
            CurrentToken = Scanner.ScanJsDocToken();
        }

        Identifier ParseJsDocIdentifierName() => CreateJsDocIdentifier(TokenIsIdentifierOrKeyword(Token));

        Identifier CreateJsDocIdentifier(bool isIdentifier)
        {
            if (!isIdentifier)
            {

                ParseErrorAtCurrentToken(Diagnostics.Identifier_expected);

                return null;
            }
            var pos = Scanner.TokenPos;
            var end2 = Scanner.TextPos;
            var result = new Identifier { Text = content.SubString(pos, end2) };

            FinishNode(result, end2);

            NextJsDocToken();

            return result;
        }
    }
}

