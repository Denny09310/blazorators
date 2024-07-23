// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

using Blazor.SourceGenerators.TypeScript.Types;

namespace Blazor.SourceGenerators.Extensions;

public static class NodeExtensions
{
    public static string GetNodeText(this INode node)
    {
        return node.GetText().ToString().Trim();
    }

    public static IEnumerable<Node> OfKind(this IEnumerable<Node> nodes, TypeScriptSyntaxKind kind)
    {
        foreach (var node in nodes)
        {
            if (node.Kind == kind) yield return node;
        }
    }
}