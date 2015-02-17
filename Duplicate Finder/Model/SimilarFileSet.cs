using System.Collections.Generic;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarFileSet : HashSet<HashSet<DupeFileInfo>>
    {

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        public HashingType HashingType { get; private set; }

        public SimilarFileSet(SortedList<IFileHash, DupeFileInfo> sortedFiles, HashingType hashingType)
        {
            _log.Debug("Building similar file sets for {0} files", sortedFiles.Count);

            HashingType = hashingType;

            IFileHash currentGroupHash = null;
            var currentFileGroup = new HashSet<DupeFileInfo>();

            foreach (var file in sortedFiles)
            {
                var currentHash = file.Value.GetOrComputeHash(hashingType);
                if (currentHash.Equals(currentGroupHash))
                {
                    _log.Trace("Hash '{0}' goes to same group '{1}'", currentHash, currentFileGroup);
                    currentFileGroup.Add(file.Value);
                }
                else
                {
                    this.Add(currentFileGroup);
                    _log.Trace("File group is full with {0} elements", currentFileGroup.Count);
                    currentFileGroup = new HashSet<DupeFileInfo>();
                    _log.Trace("Hash '{0}' goes to new group '{1}'", currentHash, currentFileGroup);
                    currentFileGroup.Add(file.Value);
                }
                
            }

            _log.Info("Found {0} different file sets for {1} files using {2} hash type", this.Count, sortedFiles.Count, hashingType);
        }
    }
}