using System;
using System.IO;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    class SizeHash : FileHash
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        private readonly long _fileSize;

        public SizeHash(FileInfo fileInfo)
        {
            _log.Debug("Computing SIZE hash for file '{}'", fileInfo.FullName);

            _fileSize = fileInfo.Length;

            _log.Debug("SIZE hash for file '{}' is: {}", fileInfo.FullName, _fileSize);
        }


    }
}
