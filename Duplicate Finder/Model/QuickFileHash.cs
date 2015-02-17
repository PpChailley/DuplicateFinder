using System;
using System.IO;
using System.Security.Cryptography;
using Gbd.Sandbox.DuplicateFinder.External;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class QuickFileHash: Sha1FileHash, IFileHash
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    

        private const int BUFFER_SIZE = 4096;
        private const int BUFFER_FIRST_HALF_SIZE = BUFFER_SIZE / 2;
        private const int BUFFER_SECOND_HALF_SIZE = BUFFER_SIZE - BUFFER_FIRST_HALF_SIZE;


        private readonly SHA1 _sha1Engine = new SHA1Cng();
        


        public QuickFileHash(FileInfo fileInfo)
        {
            Log.Debug("Creating QUICK hash for file '{0}' with size {1} bytes", fileInfo.Name, fileInfo.Length);

            var f = fileInfo.OpenRead();
            byte[] truncatedData = new byte[BUFFER_SIZE];

            if (fileInfo.Length < BUFFER_SIZE)
            {
                Log.Trace("Using full file for hashing");

                var readSize = f.Read(truncatedData, 0, BUFFER_SIZE);
                if (readSize != fileInfo.Length)
                {
                    throw new IOException(String.Format("Unexpected size read from file '{0}': {1} bytes from a file with size {2}",
                        fileInfo.Name, readSize, fileInfo.Length));
                }
            }
            else
            {
                Log.Trace("Using first and last chunk for hashing");

                var readSize = f.Read(truncatedData, 0, BUFFER_FIRST_HALF_SIZE);
                if (readSize != BUFFER_FIRST_HALF_SIZE)
                {
                    throw new IOException("Unexpected size read from file '{filename}': {size} bytes, expecting constant value {value}"
                        .Inject(new { filename = fileInfo.Name, size = readSize, value = BUFFER_FIRST_HALF_SIZE }));
                }

                long lastPartOffset =  fileInfo.Length - BUFFER_SECOND_HALF_SIZE - 1;
                f.Seek(-BUFFER_SECOND_HALF_SIZE, SeekOrigin.End);
                readSize = f.Read(truncatedData, BUFFER_FIRST_HALF_SIZE, BUFFER_SECOND_HALF_SIZE);
                if (readSize != BUFFER_SECOND_HALF_SIZE)
                {
                    throw new IOException(String.Format("Unexpected size read from file '{0}': {1} bytes at offset {2} from size {3}, expecting constant value {4}",
                        fileInfo.Name, readSize, lastPartOffset, fileInfo.Length, BUFFER_SECOND_HALF_SIZE));
                }
            }

            Hash = _sha1Engine.ComputeHash(truncatedData);

            Log.Debug("QUICK hash for '{0}' is: {1}", fileInfo.Name, Convert.ToBase64String(Hash));
        }



    }
}
