using System;
using System.IO;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SizeHash : FileHash , IFileHash
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    

        private readonly long _fileSize;

        public SizeHash(FileInfo fileInfo)
        {
            Log.Debug("Computing SIZE hash for file '{0}'", fileInfo.Name);

            _fileSize = fileInfo.Length;

            Log.Debug("SIZE hash for file '{0}' is: {1}", fileInfo.Name, _fileSize);
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
