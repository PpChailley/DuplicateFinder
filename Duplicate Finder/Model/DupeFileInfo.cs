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

        public SizeHash SizeHash { get; private set; }
        public QuickHash QuickHash { get; private set; }
        public FullHash FullHash { get; private set; }


        public DupeFileInfo(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }



        public void ComputeSizeHash()
        {
            SizeHash = new SizeHash(_fileInfo);
        }
        public void ComputeQuickHash()
        {
            QuickHash = new QuickHash(_fileInfo);
        }
        public void ComputeFullHash()
        {
            FullHash = new FullHash(_fileInfo);
        }


        public object GetHash(HashingType hashingType)
        {
            switch (hashingType)
            {
                case HashingType.SizeHashing:
                    return SizeHash;
                case HashingType.QuickHashing:
                    return QuickHash;
                case HashingType.Fullhashing:
                    return FullHash;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unsupported Hashing mode {0}", hashingType));
            }
        }
    }
}
