using System;
using System.IO;
using System.Security.Cryptography;
using Gbd.Sandbox.DuplicateFinder.Model;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class FullFileHash: Sha1FileHash, IFileHash
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        private readonly SHA1 _sha1 = new SHA1Cng();


        public FullFileHash(FileInfo fileInfo)
        {
            _log.Trace("Creating FULL hash for '{0}'", fileInfo.Name);
            Hash = _sha1.ComputeHash(new StreamReader(fileInfo.FullName).BaseStream);
            _log.Debug("FULL Hash for '{0}' is: {1}", fileInfo.Name, Convert.ToBase64String(Hash));
        }

    }
}
