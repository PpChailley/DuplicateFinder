using System.IO;
using System.Security.Cryptography;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    class FullHash
    {

        private readonly SHA1 _sha1 = new SHA1Cng();
        private byte[] _hash;


        public FullHash(FileInfo fileInfo)
        {
            _hash = _sha1.ComputeHash(new StreamReader(fileInfo.Name).BaseStream);
        }



    }
}
