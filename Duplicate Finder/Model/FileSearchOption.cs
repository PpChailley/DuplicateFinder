using System;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    [Flags]
    public enum FileSearchOption :  int
    {
        NoOption = 0,
        BgComputeHash = 1,
    }
}