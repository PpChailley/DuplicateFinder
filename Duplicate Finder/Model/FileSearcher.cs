using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class FileSearcher
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        private DirectoryInfo _baseDirectory;
        private ICollection<DupeFileInfo> _fileList = new HashSet<DupeFileInfo>();

        // TODO: make things more flexible
        //private IDictionary<Type, bool> _allHashesOfCurrentTypeDone;
        private bool _allSizeHashesKnown;
        private bool _allQuickHashesKnown;
        private bool _allFullHashesKnown;
        private SimilarFileSet _similars;


        public FileSearcher Reset()
        {
            _baseDirectory = null;
            _fileList = new List<DupeFileInfo>();

            _allFullHashesKnown = false;
            _allQuickHashesKnown = false;
            _allSizeHashesKnown = false;
            
            _log.Info("Reset of FileSearcher successful");

            return this;
        }

        public FileSearcher SetDirectory(string newDirectory)
        {
            _baseDirectory = new DirectoryInfo(newDirectory);

            _log.Info("FileSearcher now working in directory '{0}'", _baseDirectory.FullName);

            return this;
        }

        public FileSearcher BuildFileList(FileSearchOption options)
        {
            _log.Info("Start building file list");

            foreach (var curFile in 
                        _baseDirectory.GetFiles("*", SearchOption.AllDirectories)
                        .Where(file => file.Exists))
            {
                var info = new DupeFileInfo(curFile);
                _fileList.Add(info);
            }

            _log.Info("Found {0} files in search directory", _fileList.Count);

            if ((options & FileSearchOption.BgComputeHash) != 0)
            {
                _log.Trace("Option BgComputeHash is set: launching BG hashing");
                // TODO: start hashing as soon as file is known (Size hashing should not generate IOs)
                this.StartBgHashing();
            }

            return this;
        }

        public FileSearcher CompareSizeHashes()
        {
            var sortedHashes = new SortedList<IFileHash, DupeFileInfo>(_fileList.Count);
            foreach (var file in _fileList)
            {
                sortedHashes.Add(file.SizeHash, file);
            }

            _similars = new SimilarFileSet(sortedHashes, HashingType.SizeHashing);

            return this;
        }

        private void StartBgHashing()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(HashAndCompare);
            // TODO: report progress delegate
            worker.RunWorkerAsync();
        }

        public void HashAndCompare(object sender, DoWorkEventArgs e)
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = "HashAndCompare";
            }

            _log.Info("Start (normally BG) routine HashAndCompare");

            ComputeSizeHashes();
            ComputeQuickHashes();
            ComputeFullHashes();



            // TODO: report progress
            // TODO: deal with cancellation
        }

        private void ComputeSizeHashes()
        {
            _log.Debug("Start computing SIZE hashes for all {0} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.SizeHash == null))
            {
                file.ComputeSizeHash();
            }

            this._allSizeHashesKnown = true;
            _log.Debug("Done computing SIZE hashes for all {0} known files", _fileList.Count);
        }

        private void ComputeQuickHashes()
        {
            _log.Debug("Start computing QUICK hashes for all {0} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.QuickHash == null))
            {
                file.ComputeQuickHash();
            }

            this._allQuickHashesKnown = true;
            _log.Debug("Done computing QUICK hashes for all {0} known files", _fileList.Count);
        }

        private void ComputeFullHashes()
        {
            _log.Debug("Start computing FULL hashes for all {0} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.FullHash == null))
            {
                file.ComputeFullHash();
            }

            this._allFullHashesKnown = true;
            _log.Debug("Done computing FULL hashes for all {0} known files", _fileList.Count);
        }
    }
}