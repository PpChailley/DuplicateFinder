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
            _log.Debug("Creating FULL hash for '{}'", fileInfo.FullName);
            _hash = _sha1.ComputeHash(new StreamReader(fileInfo.Name).BaseStream);
            _log.Debug("FULL Hash for '{}' is: {}", fileInfo.FullName, _hash);
        }



    }
}
