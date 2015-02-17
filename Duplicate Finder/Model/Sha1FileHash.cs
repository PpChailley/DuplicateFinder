namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public abstract class Sha1FileHash: FileHash
    {

        protected byte[] Hash;

        public override int CompareTo(IFileHash other)
        {
            // TODO: Unit test this method

            QuickFileHash that = (QuickFileHash)other;

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

    }
}