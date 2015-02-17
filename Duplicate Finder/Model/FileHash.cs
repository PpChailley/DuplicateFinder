using System.Collections.Generic;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public abstract class FileHash: IFileHash
    {
        public abstract int CompareTo(IFileHash other);
    }
}