// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.CodeAnalysis;
using Xunit;

namespace Blazor.SourceGenerators.Tests;

public class JavaScriptInteropTests : GeneratorBaseUnitTests
{
    public override IEnumerable<IIncrementalGenerator> SourceGenerators => [new JavaScriptInteropGenerator()];


    [Fact]
    public void Basic()
    {
        // TODO: write test
        var sourceCode = """

            using System;
            using Microsoft.JSInterop.Attributes;

            #pragma warning disable 649

            [JSAutoInterop(
                TypeName = "Geolocation",
                Implementation = "window.navigator.geolocation",
                Url = "https://developer.mozilla.org/docs/Web/API/Geolocation")]
            public partial interface IGeolocationService;

            """;

        _ = GetRunResult(sourceCode);
    }
}
