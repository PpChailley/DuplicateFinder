using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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


        public FileSearcher Reset()
        {
            _baseDirectory = null;
            _fileList = null;

            _allFullHashesKnown = false;
            _allQuickHashesKnown = false;
            _allSizeHashesKnown = false;
            
            _log.Info("Reset of FileSearcher successful");

            return this;
        }

        public FileSearcher SetDirectory(string newDirectory)
        {
            _baseDirectory = new DirectoryInfo(newDirectory);

            _log.Info("FileSearcher now working in directory '{}'", _baseDirectory.FullName);

            return this;
        }

        public FileSearcher BuildFileList(Options options)
        {
            _log.Info("Start building file list");

            foreach (var curFile in 
                        _baseDirectory.GetFiles("*", SearchOption.AllDirectories)
                        .Where(file => file.Exists))
            {
                _fileList.Add(new DupeFileInfo(curFile));
            }

            _log.Info("Found {} files in search directory", _fileList.Count);

            if ((options & Options.BgComputeHash) != 0)
            {
                _log.Trace("Option BgComputeHash is set: launching BG hashing");
                // TODO: start hashing as soon as file is known (Size hashing should not generate IOs)
                this.StartBgHashing();
            }

            return this;
        }

        private void StartBgHashing()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(ComputeAllHashes);
            // TODO: report progress delegate
            worker.RunWorkerAsync();
        }

        public void ComputeAllHashes(object sender, DoWorkEventArgs e)
        {
            ComputeSizeHashes();
            ComputeQuickHashes();
            ComputeFullHashes();


            // TODO: report progress
            // TODO: deal with cancellation
            throw new NotImplementedException();
        }

        private void ComputeSizeHashes()
        {
            _log.Debug("Start computing SIZE hashes for all {} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.SizeHashKnown == false))
            {
                file.ComputeSizeHash();
            }

            this._allSizeHashesKnown = true;
        }

        private void ComputeQuickHashes()
        {
            _log.Debug("Start computing QUICK hashes for all {} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.QuickHashKnown == false))
            {
                file.ComputeQuickHash();
            }

            this._allQuickHashesKnown = true;
        }

        private void ComputeFullHashes()
        {
            _log.Debug("Start computing FULL hashes for all {} known files", _fileList.Count);

            foreach (var file in _fileList.Where(f => f.FullHashKnown == false))
            {
                file.ComputeFullHash();
            }

            this._allFullHashesKnown = true;
        }

        [Flags]
        public enum Options :  int
        {
            NoOption = 0,
            BgComputeHash = 1,
        }
    }
}