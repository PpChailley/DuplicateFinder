using System;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    [Flags]
    internal enum FileSearchOption :  int
    {
        NoOption = 0,
        BgComputeHash = 1,
    }
}