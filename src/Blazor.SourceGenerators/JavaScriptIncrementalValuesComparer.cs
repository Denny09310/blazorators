// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Blazor.SourceGenerators;

internal class JavaScriptIncrementalValuesComparer : IEqualityComparer<InterfaceDeclarationDetails>
{
    public bool Equals(InterfaceDeclarationDetails x, InterfaceDeclarationDetails y)
    {
        return x.InterfaceName.Equals(y.InterfaceName);
    }

    public int GetHashCode(InterfaceDeclarationDetails obj)
    {
        return obj.InterfaceName.GetHashCode();
    }
}
