using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class DupeFinder
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    


        public static DupeFinder Finder  = new DupeFinder();
        private readonly FileSearcher _searcher = new FileSearcher();


        public DirectoryInfo SearchPath;
        public SearchOptions SearchOptions;
        public IList<HashingType> HashingSequence = new List<HashingType>
            {
                // HashingType.SizeHashing, Size hashing is done at build time (no IO cost)
                HashingType.QuickHashing,
                HashingType.FullHashing
            };

        

        private SimilarityMap _similars;
        private IDictionary<HashingType, bool> _allHashesDoneByType;


        public DupeFinder Initialize(String searchDirectoryPath)
        {
            this.SearchPath = new DirectoryInfo(searchDirectoryPath);

            _allHashesDoneByType = new Dictionary<HashingType, bool>();
            foreach (var hashingType in HashingSequence)
            {
                _allHashesDoneByType.Add(hashingType, false);
            }

            _searcher.Reset();
            _searcher.SetDirectory(SearchPath);

            return this;
        }

        public DupeFinder SearchForFiles()
        {
            _searcher.BuildFileList();
            return this;
        }

        public DupeFinder DoHashing()
        {
            foreach (var hashingType in HashingSequence)
            {
                ComputeHashes(hashingType);
            }
            return this;
        }

        private void ComputeHashes(HashingType hashingType)
        {
            var files = _searcher.FileList;

            Log.Debug("Start computing {1} hashes. {0} known files yet", files.Count, hashingType);

            foreach (var file in files.Where(f => f.GetOrComputeHash(hashingType) == null))
            {
                file.GetOrComputeHash(hashingType);
            }

            _allHashesDoneByType[hashingType] = true;
            Log.Debug("Done computing {1} hashes for all {0} files", files.Count, hashingType);
        }

        public DupeFinder CompareHashes(HashingType hashingType)
        {
            Log.Debug("Start building sorted list for {0} hashing", hashingType);

            _similars = new SimilarityMap(_searcher.FileList, hashingType);

            return this;
        }


        public DupeFinder DoCompareHashedResults()
        {
            CompareHashes(HashingType.SizeHashing);

            foreach (var hashingType in HashingSequence)
            {
                _similars.Refine(hashingType);
            }


            Log.Debug("Final (refined) {0} is: {1}", _similars.GetType().Name, _similars.ToString(1));
            return this;
        }
    }
}
