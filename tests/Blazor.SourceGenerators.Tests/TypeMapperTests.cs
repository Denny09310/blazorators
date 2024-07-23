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
        Dictionary<string, DeclarationStatement> GetTypeMap(bool timePenalty)
        {
            var startingTimestamp = Stopwatch.GetTimestamp();
            var sut = new DependencyMapBuilder().Build;

            var typeMap = sut("Geolocation");

            Assert.NotEmpty(typeMap);
            var elapsedMilliseconds = GetElapsedMilliseconds(startingTimestamp);

            if (timePenalty)
            {
                Assert.True(
                    elapsedMilliseconds < 1_000,
                    $"Expected operation to complete in under 1,000ms, but took {elapsedMilliseconds}ms.");
            }

            return typeMap;
        }

        double GetElapsedMilliseconds(long startingTimestamp)
        {
            var endingTimestamp = Stopwatch.GetTimestamp();
            return (endingTimestamp - startingTimestamp) / (double)Stopwatch.Frequency * 1_000;
        }

        void ValidateTypeMap(Dictionary<string, DeclarationStatement> typeMap)
        {
            IEnumerable<string> expectedTypes =
            [
                "Geolocation",
                "PositionCallback",
                "PositionErrorCallback",
                "PositionOptions",
                "GeolocationPosition",
                "GeolocationPositionError",
                "GeolocationCoordinates"
            ];

            foreach (var type in expectedTypes)
            {
                Assert.True(typeMap.ContainsKey(type), $"Type '{type}' was not found in the type map.");
            }
        }

        var typeMap = GetTypeMap(timePenalty: false);
        ValidateTypeMap(typeMap);
    }
}