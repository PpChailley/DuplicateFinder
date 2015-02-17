using System.Collections.Generic;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarFileSet : HashSet<HashSet<DupeFileInfo>>
    {

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        public HashingType HashingType { get; private set; }

        public SimilarFileSet(ICollection <DupeFileInfo> files, HashingType hashingType)
        {
            this.HashingType = hashingType;

            var sortedFiles = BuildFileSortedList(files, hashingType);
            BuildFileGroups(hashingType, sortedFiles);

        }

        private void BuildFileGroups(HashingType hashingType, IList<DupeFileInfo> sortedFiles)
        {
            _log.Debug("Building similar file sets for {0} files", sortedFiles.Count);

            IFileHash currentGroupHash = null;
            var currentFileGroup = new HashSet<DupeFileInfo>();

            foreach (var file in sortedFiles)
            {
                var currentHash = file.GetOrComputeHash(hashingType);
                if (currentHash.Equals(currentGroupHash))
                {
                    _log.Trace("Hash '{0}' goes to same group '{1}'", currentHash, currentFileGroup);
                    currentFileGroup.Add(file);
                }
                else
                {
                    Add(currentFileGroup);
                    _log.Trace("File group is full with {0} elements", currentFileGroup.Count);
                    currentFileGroup = new HashSet<DupeFileInfo>();
                    _log.Trace("Hash '{0}' goes to new group '{1}'", currentHash, currentFileGroup);
                    currentFileGroup.Add(file);
                    currentGroupHash = currentHash;
                }
            }

            _log.Info("Found {0} different file sets for {1} files using {2} hash type", this.Count, sortedFiles.Count, hashingType);

        }

        private static IList<DupeFileInfo> BuildFileSortedList(ICollection<DupeFileInfo> files, HashingType hashingType)
        {
            _log.Debug("Creating sorted list of files, using hashing {0}", hashingType);


            var sortedFiles = new List<DupeFileInfo>(files);
            sortedFiles.Sort((a, b) => a.GetOrComputeHash(hashingType).CompareTo(b.GetOrComputeHash(hashingType)));

            _log.Debug("Created sorted list of files, using hashing {0}", hashingType);
            return sortedFiles;
        }
    }
}