﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace Gbd.Sandbox.DuplicateFinder.Model
{

    class QuickHash
    {

        private const int BUFFER_SIZE = 4096;
        private const int BUFFER_FIRST_HALF_SIZE = BUFFER_SIZE / 2;
        private const int BUFFER_SECOND_HALF_SIZE = BUFFER_SIZE - BUFFER_FIRST_HALF_SIZE;


        private readonly SHA1 _sha1 = new SHA1Cng();
        private byte[] _hash;
        


        public QuickHash(FileInfo fileInfo)
        {

            var f = fileInfo.OpenRead();
            
            byte[] truncatedData = new byte[BUFFER_SIZE];

            if (fileInfo.Length < BUFFER_SIZE)
            {
                var readSize = f.Read(truncatedData, 0, BUFFER_SIZE);
                if (readSize != fileInfo.Length)
                {
                    throw new IOException(String.Format("Unexpected size read from file '{}': {} bytes from a file with size {}", 
                        fileInfo.FullName, readSize, fileInfo.Length));
                }
            }
            else
            {
                var readSize = f.Read(truncatedData, 0, BUFFER_FIRST_HALF_SIZE);
                if (readSize != BUFFER_FIRST_HALF_SIZE)
                {
                    throw new IOException(String.Format("Unexpected size read from file '{}': {} bytes, expecting constant value {}", 
                        fileInfo.FullName, readSize, BUFFER_FIRST_HALF_SIZE));
                }

                long lastPartOffset =  fileInfo.Length - BUFFER_SECOND_HALF_SIZE;
                readSize = f.Read(truncatedData, (int) lastPartOffset, BUFFER_SECOND_HALF_SIZE);
                if (readSize != BUFFER_SECOND_HALF_SIZE)
                {
                    throw new IOException(String.Format("Unexpected size read from file '{}': {} bytes at offset {} from size {}, expecting constant value {}", 
                        fileInfo.FullName, readSize, lastPartOffset, fileInfo.Length, BUFFER_SECOND_HALF_SIZE));
                }
            }

            _hash = _sha1.ComputeHash(truncatedData);
        }

     
    }
}