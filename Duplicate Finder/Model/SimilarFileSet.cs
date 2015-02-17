using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
                if (currentHash.CompareTo(currentGroupHash) == 0)
                {
                    _log.Trace("Hash '{0}' goes to same group '{1}'", 
                        currentHash, 
                        GCHandle.ToIntPtr(GCHandle.Alloc(currentFileGroup, GCHandleType.WeakTrackResurrection)).ToInt32());
                    currentFileGroup.Add(file);
                }
                else
                {
                    _log.Trace("Group ({1}) is full with {0} similar files",
                        currentFileGroup.Count,
                        GCHandle.ToIntPtr(GCHandle.Alloc(currentFileGroup, GCHandleType.WeakTrackResurrection)).ToInt32());

                    Add(currentFileGroup);
                    currentFileGroup = new HashSet<DupeFileInfo>();

                    _log.Trace("Hash '{0}' goes to new group '{1}'", 
                        currentHash, 
                        GCHandle.ToIntPtr(GCHandle.Alloc(currentFileGroup, GCHandleType.WeakTrackResurrection)).ToInt32());
                    currentFileGroup.Add(file);
                    currentGroupHash = currentHash;
                }
            }
            _log.Trace("Last Group ({0}) has last element - Commiting group with {1} similar files",
                GCHandle.ToIntPtr(GCHandle.Alloc(currentFileGroup, GCHandleType.WeakTrackResurrection)).ToInt32(),
                currentFileGroup.Count);
            Add(currentFileGroup);

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