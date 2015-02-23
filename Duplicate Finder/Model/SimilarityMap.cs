using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using Gbd.Sandbox.DuplicateFinder.Tools;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SimilarityMap
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        internal HashSet<FileEquivalenceClass> Map = new HashSet<FileEquivalenceClass>();
        internal ICollection<SimilarityMap> RefinedMaps = null;

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
                if (RefinedMaps == null && Map != null)
                    return 0;
                else if (RefinedMaps != null && Map == null)
                    return RefinedMaps.First().Depth + 1;
                else
                    throw new InvalidOperationException("Similarity map should has none or both of map and refined maps");
            }
        }


        public int Count
        {
            get
            {
                if (Depth == 0 && Map != null)
                    return Map.Sum(equivalenceClass => equivalenceClass.Count);

                if (Map == null && RefinedMaps != null)
                    return RefinedMaps.Sum(map => map.Count);

                throw new InvalidOperationException("Inconsistent state for SimilarityMap: Data have to be refined or not refined");
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
            FileEquivalenceClass currentFileGroup = null;

            foreach (var file in sortedFiles)
            {
                var currentHash = file.GetOrComputeHash(hashingType);

                if (currentHash.CompareTo(currentGroupHash) == 0)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    // currentFileGroup is null on first iteration only, when the if is false
                    currentFileGroup.Add(file);
                }
                else
                {
                    if (currentFileGroup != null)
                        Map.Add(currentFileGroup);
                    currentFileGroup = new FileEquivalenceClass(hashingType) { file };
                    currentGroupHash = currentHash;
                }
            }
            Map.Add(currentFileGroup);

            Log.Info("Found {0} different file sets for {1} files using {2} hash type", Map.Count, sortedFiles.Count, hashingType);
            Log.Debug("SimilarityMap just created is: {0}", this);

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

            if (RefinedMaps != null)
            {
                Log.Debug("Current similarity map is already refined. Refining children");
                foreach (var refinedMap in RefinedMaps)
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
            Log.Debug("Refining all {0} FileEquivalenceClass into SimilarityMap", Map.Count);


            RefinedMaps = new HashSet<SimilarityMap>();

            foreach (var groupToRefine in Map)
            {
                RefinedMaps.Add(new SimilarityMap(groupToRefine, refinedHashingType));
            }

            this.Map = null;
        }


        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int indentationLevel)
        {
            StringBuilder sb = new StringBuilder(200);

            var data = new
                        {
                            firstNewLine = indentationLevel == 1 ? Environment.NewLine : String.Empty, 
                            newLine = indentationLevel != 0 ? Environment.NewLine : String.Empty, 
                            indentOut = indentationLevel != 0  ? new String('\t', indentationLevel): String.Empty, 
                            indentIn = indentationLevel != 0  ? new String('\t', indentationLevel + 1): String.Empty,
                            Type = this.GetType().Name, 
                            hash = HashingType,
                            depth = this.Depth,
                            count = this.Count,
                        };




            sb.Append( "{firstNewLine}{indentOut}{Type} " + "[{hash}, depth={depth}, count={count}] " +
                        "{newLine}{indentOut}({newLine}");


            if (RefinedMaps == null)
            {
                sb.Append("{indentIn}[Not Refined]{newLine}");
            }
            else
            {
                foreach (var map in RefinedMaps)
                {
                    sb.Append(map.ToString(indentationLevel + 1));
                }
            }

            
            sb.Append(indentationLevel == 0 ? " | " : string.Empty);
            

            if (Map == null)
            {
                sb.Append("{indentIn}[No Equivalence Class]{newLine}");
            }
            else
            {
                foreach (FileEquivalenceClass eqClass in Map)
                {
                    sb.Append("{indentIn}[" + eqClass.HashingType + ", count=" + eqClass.Count + " - ");
                    foreach (var file in eqClass)
                    {
                        sb.AppendFormat("{0} , ", file.FileInfo.Name);
                    }
                    sb.Append("] {newLine}");
                }
            }


            sb.Append("{indentOut}){newLine}");

            return sb.ToString().Inject(data);
        }


    }
}