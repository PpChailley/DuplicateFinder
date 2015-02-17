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
        private QuickHash _quickHash;
        private FullHash _fullHash;


        public DupeFileInfo(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }



        public void ComputeSizeHash()
        {
            _sizeHash = new SizeHash(_fileInfo);
        }
        public void ComputeQuickHash()
        {
            _quickHash = new QuickHash(_fileInfo);
        }
        public void ComputeFullHash()
        {
            _fullHash = new FullHash(_fileInfo);
        }



        public IFileHash GetOrComputeHash(HashingType hashingType)
        {
            switch (hashingType)
            {
                case HashingType.Fullhashing:
                    return _fullHash ?? (_fullHash = new FullHash(_fileInfo));

                case HashingType.QuickHashing:
                    return _quickHash ?? (_quickHash = new QuickHash(_fileInfo));

                case HashingType.SizeHashing:
                    return _sizeHash ?? (_sizeHash = new SizeHash(_fileInfo));

                default:
                    throw new InvalidEnumArgumentException(String.Format("Unsupported ProcessHashing mode {0}", hashingType));

            }
        }
    }
}
