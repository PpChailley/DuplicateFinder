using System;

namespace Gbd.Sandbox.DuplicateFinder.Model.Hashing
{
    public abstract class Sha1FileHash: FileHash
    {

        protected byte[] Hash;

        public override int CompareTo(IFileHash other)
        {
            if (other == null)
                return -1;
            
            // TODO: Unit test this method

            Sha1FileHash that = (Sha1FileHash)other;

            int index = 0;
            foreach (var b in that.Hash)
            {
                if (Hash.Length < index) return -1;

                int byteComparisonResult = b.CompareTo(Hash[index++]);

                if (byteComparisonResult != 0)
                    return byteComparisonResult;
            }

            return that.Hash.Length - this.Hash.Length;
        }



        public override string ToString()
        {
            return String.Format("{0} : {1}", 
                    GetType().Name,
                    Hash == null ? "null" : Convert.ToBase64String(Hash));
        }


    }
}