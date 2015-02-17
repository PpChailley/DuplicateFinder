using System.Collections.Generic;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public interface IFileHash
    {
        int CompareTo(IFileHash other);
    }
}