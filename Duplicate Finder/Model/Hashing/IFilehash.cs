namespace Gbd.Sandbox.DuplicateFinder.Model.Hashing
{
    public interface IFileHash
    {
        int CompareTo(IFileHash other);

    }
}