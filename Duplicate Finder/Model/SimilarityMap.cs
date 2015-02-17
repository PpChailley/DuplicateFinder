using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarityMap
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private HashSet<FileEquivalenceClass> _map = new HashSet<FileEquivalenceClass>();
        private ICollection<SimilarityMap> _refinedMaps = null;

        public HashingType HashingType { get; private set; }

        /// <summary>
        /// Gets the refinement depth of current map
        /// 0 = Not refined, ie contains FileEquivalenceClass'es
        /// 2 = Refined twice, using a total of 3 different hashing algorithms
        /// </summary>
        public int Depth
        {
            get
            {
                if (_refinedMaps == null && _map != null)
                    return 0;
                else if (_refinedMaps != null && _map == null)
                    return _refinedMaps.First().Depth + 1;
                else
                    throw new InvalidOperationException("Similarity map should has none or both of map and refined maps");
            }
        }

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
            var currentFileGroup = new FileEquivalenceClass(hashingType);

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
                    currentFileGroup = new FileEquivalenceClass(hashingType) { file };
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

        public void Refine(HashingType refinedHashingType)
        {
            Log.Info("Refining current similarity map ({0}) with {1}", HashingType, refinedHashingType);

            if (_refinedMaps != null)
            {
                Log.Debug("Current similarity map is already refined. Refining children");
                foreach (var refinedMap in _refinedMaps)
                {
                    refinedMap.Refine(refinedHashingType);
                }
            }
            else
            {
                if (refinedHashingType.Equals(HashingType))
                    throw new ArgumentException(String.Format("This similarity map has already used hashing algorithm {0}", refinedHashingType));

                ProcessRefining(refinedHashingType);
            }

            Log.Info("Refining of {0} complete with {1}", HashingType, refinedHashingType);
        }

        private void ProcessRefining(HashingType refinedHashingType)
        {
            Log.Debug("Refining all {0} FileEquivalenceClass into SimilarityMap", _map.Count);

            _refinedMaps = new HashSet<SimilarityMap>();

            foreach (var groupToRefine in _map)
            {
                _refinedMaps.Add(new SimilarityMap(groupToRefine, refinedHashingType));
            }

            this._map = null;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(200);

            sb.Append(String.Format("{0} [{1}, depth={2}] (", this.GetType(), HashingType, this.Depth));

            foreach (var map in _refinedMaps)
            {
                sb.Append(map.ToString());
            }

            foreach (FileEquivalenceClass eqClass in _map)
            {
                sb.Append(String.Format("[Class {0}, count={1}] ", eqClass.HashingType, eqClass.Count));
            }

            sb.Append(")");

            return sb.ToString();
        }


    }
}