using System;
using System.IO;
using System.Security.Cryptography;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model.Hashing
{
    public class FullFileHash: Sha1FileHash, IFileHash
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    

        private readonly SHA1 _sha1 = new SHA1Cng();


        public FullFileHash(FileInfo fileInfo)
        {
            Log.Trace("Creating FULL hash for '{0}'", fileInfo.Name);
            Hash = _sha1.ComputeHash(new StreamReader(fileInfo.FullName).BaseStream);
            Log.Debug("FULL Hash for '{0}' is: {1}", fileInfo.Name, Convert.ToBase64String(Hash));
        }

    }
}
