using System.Collections.Generic;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarFileSet : HashSet<HashSet<DupeFileInfo>>
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    

        public HashingType HashingType { get; private set; }

        public SimilarFileSet(ICollection <DupeFileInfo> files, HashingType hashingType)
        {
            this.HashingType = hashingType;

            var sortedFiles = BuildFileSortedList(files, hashingType);
            BuildFileGroups(hashingType, sortedFiles);

        }

        private void BuildFileGroups(HashingType hashingType, IList<DupeFileInfo> sortedFiles)
        {
            Log.Debug("Building similar file sets for {0} files", sortedFiles.Count);

            IFileHash currentGroupHash = null;
            var currentFileGroup = new HashSet<DupeFileInfo>();

            foreach (var file in sortedFiles)
            {
                var currentHash = file.GetOrComputeHash(hashingType);

                if (currentHash.CompareTo(currentGroupHash) == 0)
                {
                    currentFileGroup.Add(file);
                }
                else
                {
                    this.Add(currentFileGroup);
                    currentFileGroup = new HashSet<DupeFileInfo> {file};
                    currentGroupHash = currentHash;
                }
            }
            this.Add(currentFileGroup);

            Log.Info("Found {0} different file sets for {1} files using {2} hash type", this.Count, sortedFiles.Count, hashingType);

        }

        private static IList<DupeFileInfo> BuildFileSortedList(ICollection<DupeFileInfo> files, HashingType hashingType)
        {
            Log.Debug("Creating sorted list of files, using hashing {0}", hashingType);


            var sortedFiles = new List<DupeFileInfo>(files);
            sortedFiles.Sort((a, b) => a.GetOrComputeHash(hashingType).CompareTo(b.GetOrComputeHash(hashingType)));

            Log.Debug("Created sorted list of files, using hashing {0}", hashingType);
            return sortedFiles;
        }
    }
}