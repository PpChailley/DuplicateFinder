using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public interface IFileHash
    {
        int CompareTo(IFileHash other);

    }
}