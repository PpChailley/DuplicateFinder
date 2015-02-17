namespace Gbd.Sandbox.DuplicateFinder.Model.Hashing
{
    public abstract class FileHash: IFileHash
    {
        public abstract int CompareTo(IFileHash other);



    }
}