using System;
using System.IO;
using System.Security.Cryptography;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Forms.Model
{
    class FullHash
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        private readonly SHA1 _sha1 = new SHA1Cng();
        private byte[] _hash;


        public FullHash(FileInfo fileInfo)
        {
            _log.Debug("Creating FULL hash for '{0}'", fileInfo.Name);
            _hash = _sha1.ComputeHash(new StreamReader(fileInfo.FullName).BaseStream);
            _log.Debug("FULL Hash for '{0}' is: {1}", fileInfo.Name, Convert.ToBase64String(_hash));
        }



    }
}
