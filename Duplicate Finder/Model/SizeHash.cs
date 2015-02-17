using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SizeHash : FileHash , IFileHash
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        private readonly long _fileSize;

        public SizeHash(FileInfo fileInfo)
        {
            _log.Debug("Computing SIZE hash for file '{0}'", fileInfo.Name);

            _fileSize = fileInfo.Length;

            _log.Debug("SIZE hash for file '{0}' is: {1}", fileInfo.Name, _fileSize);
        }


        public override int CompareTo(IFileHash other)
        {
            if (other == null)
                return -1;

            var you = (SizeHash) other;
            int myFileSize = (int)_fileSize;
            int yourFileSize = (int)you._fileSize;

            return (yourFileSize - myFileSize);
        }


        public override string ToString()
        {
            return String.Format("{1} : {0}", _fileSize, this.GetType().Name);
        }

    }
}
