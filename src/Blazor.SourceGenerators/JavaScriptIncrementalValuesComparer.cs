// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators;

internal class JavaScriptIncrementalValuesComparer : IEqualityComparer<InterfaceDeclarationDetails>
{
    public bool Equals(InterfaceDeclarationDetails x, InterfaceDeclarationDetails y)
    {
        return x.InterfaceDeclaration.Identifier.Equals(y.InterfaceDeclaration.Identifier);
    }

    public int GetHashCode(InterfaceDeclarationDetails obj)
    {
        return obj.InterfaceDeclaration.Identifier.GetHashCode();
    }
}
