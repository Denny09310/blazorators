﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace TypeScript.TypeConverter.Parsers;

public record class ParserResult<T>(ParserResultStatus Status) where T : class
{
    public T? Value { get; init; } = default!;

    public string? Error { get; init; } = default!;
}
