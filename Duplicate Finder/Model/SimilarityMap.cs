using System.Collections.Generic;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarityMap
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private HashSet<SimilarFileGroup> _map;

        public HashingType HashingType { get; private set; }

        public SimilarityMap(IEnumerable<DupeFileInfo> files, HashingType hashingType)
        {
            this.HashingType = hashingType;

            var sortedFiles = BuildFileSortedList(files, hashingType);
            BuildFileGroups(hashingType, sortedFiles);

        }

        private void BuildFileGroups(HashingType hashingType, IList<DupeFileInfo> sortedFiles)
        {
            Log.Debug("Building similar file sets for {0} files", sortedFiles.Count);

            IFileHash currentGroupHash = null;
            var currentFileGroup = new SimilarFileGroup();

            foreach (var file in sortedFiles)
            {
                var currentHash = file.GetOrComputeHash(hashingType);

                if (currentHash.CompareTo(currentGroupHash) == 0)
                {
                    currentFileGroup.Add(file);
                }
                else
                {
                    _map.Add(currentFileGroup);
                    currentFileGroup = new SimilarFileGroup { file };
                    currentGroupHash = currentHash;
                }
            }
            _map.Add(currentFileGroup);

            Log.Info("Found {0} different file sets for {1} files using {2} hash type", _map.Count, sortedFiles.Count, hashingType);

        }

        private static IList<DupeFileInfo> BuildFileSortedList(IEnumerable<DupeFileInfo> files, HashingType hashingType)
        {
            Log.Debug("Creating sorted list of files, using hashing {0}", hashingType);


            var sortedFiles = new List<DupeFileInfo>(files);
            sortedFiles.Sort((a, b) => a.GetOrComputeHash(hashingType).CompareTo(b.GetOrComputeHash(hashingType)));

            Log.Debug("Created sorted list of files, using hashing {0}", hashingType);
            return sortedFiles;
        }
    }
}