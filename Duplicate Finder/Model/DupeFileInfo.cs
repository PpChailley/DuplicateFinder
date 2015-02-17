using System;
using System.ComponentModel;
using System.IO;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class DupeFileInfo
    {
        public readonly FileInfo FileInfo;

        private SizeHash _sizeHash;
        private QuickFileHash _quickFileHash;
        private FullFileHash _fullFileHash;

        

        public DupeFileInfo(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }


        public IFileHash GetOrComputeHash(HashingType hashingType)
        {
            lock (this)
            {
                switch (hashingType)
                {
                    case HashingType.FullHashing:
                        return _fullFileHash ?? (_fullFileHash = new FullFileHash(FileInfo));

                    case HashingType.QuickHashing:
                        return _quickFileHash ?? (_quickFileHash = new QuickFileHash(FileInfo));

                    case HashingType.SizeHashing:
                        return _sizeHash ?? (_sizeHash = new SizeHash(FileInfo));

                    default:
                        throw new InvalidEnumArgumentException(String.Format("Unsupported ProcessHashing mode {0}",
                            hashingType));

                }
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", this.GetType().Name, GetHashCode());
        }
    }
}
