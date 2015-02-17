using System;
using System.ComponentModel;
using System.IO;
using System.Security.Policy;
using Gbd.Sandbox.DuplicateFinder.Model;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class DupeFileInfo
    {
        readonly FileInfo _fileInfo;

        private SizeHash _sizeHash;
        private QuickFileHash _quickFileHash;
        private FullFileHash _fullFileHash;


        public DupeFileInfo(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }


        public IFileHash GetOrComputeHash(HashingType hashingType)
        {
            switch (hashingType)
            {
                case HashingType.Fullhashing:
                    return _fullFileHash ?? (_fullFileHash = new FullFileHash(_fileInfo));

                case HashingType.QuickHashing:
                    return _quickFileHash ?? (_quickFileHash = new QuickFileHash(_fileInfo));

                case HashingType.SizeHashing:
                    return _sizeHash ?? (_sizeHash = new SizeHash(_fileInfo));

                default:
                    throw new InvalidEnumArgumentException(String.Format("Unsupported ProcessHashing mode {0}", hashingType));

            }
        }
    }
}
