// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using Blazor.SourceGenerators.Types;
using Blazor.SourceGenerators.TypeScript.Types;
using Xunit;

namespace Blazor.SourceGenerators.Tests;

public class TypeMapperTests
{
    [Fact]
    public void TypeMapperCorrectlyMapsKnownTypeMap()
    {
        static Dictionary<string, DeclarationStatement> GetTypeMapWithPotential(bool timePenalty)
        {
            var mapper = new DependencyMapBuilder();

            var startingTimestamp = Stopwatch.GetTimestamp();
            var sut = mapper.Build;

            // The implementation: window.navigator.geolocation
            sut("Geolocation");
            var typeMap = mapper.Root;

            Assert.NotEqual(default, typeMap);
            var elapsedTimestamp = Stopwatch.GetElapsedTime(startingTimestamp);

            // This needs to take less than a second.
            // But only fail the test if there is a time penalty.
            Assert.True(
                condition: !timePenalty ||
                elapsedTimestamp.TotalMilliseconds < 1_000, $"""
                condition: timePenalty is {!timePenalty}
                or took longer than 1,000ms {elapsedTimestamp.TotalMilliseconds < 1_000}.
                """);

            return typeMap.ToDictionary();
        }

        var typeMap = GetTypeMapWithPotential(timePenalty: false);

        Assert.NotNull(typeMap["Geolocation"]);
        Assert.NotNull(typeMap["PositionCallback"]);
        Assert.NotNull(typeMap["PositionErrorCallback"]);
        Assert.NotNull(typeMap["PositionOptions"]);
        Assert.NotNull(typeMap["GeolocationPosition"]);
        Assert.NotNull(typeMap["GeolocationPositionError"]);
        Assert.NotNull(typeMap["GeolocationCoordinates"]);
    }
}
