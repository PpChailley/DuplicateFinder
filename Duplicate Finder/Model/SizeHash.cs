using System;
using System.IO;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    class SizeHash : FileHash
    {

        private readonly long _fileSize;

        public SizeHash(FileInfo fileInfo)
        {
            _fileSize = fileInfo.Length;
        }


    }
}
