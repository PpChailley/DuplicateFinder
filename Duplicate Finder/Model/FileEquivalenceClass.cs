using System.Collections.Generic;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class FileEquivalenceClass: HashSet<DupeFileInfo>
    {

        public HashingType HashingType;

        public FileEquivalenceClass(HashingType hashingType)
        {
            HashingType = hashingType;
        }

        private FileEquivalenceClass()
        {
        }

    }
}