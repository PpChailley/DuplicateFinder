using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class FileSearcher
    {

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
            
            return this;
        }

        public FileSearcher SetDirectory(string newDirectory)
        {
            _baseDirectory = new DirectoryInfo(newDirectory);
            return this;
        }

        public FileSearcher BuildFileList(Options options)
        {
            foreach (var curFile in 
                        _baseDirectory.GetFiles("*", SearchOption.AllDirectories)
                        .Where(file => file.Exists))
            {
                _fileList.Add(new DupeFileInfo(curFile));
            }

            if ((options & Options.BgComputeHash) != 0)
            {
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
            foreach (var file in _fileList.Where(f => f.SizeHashKnown == false))
            {
                file.ComputeSizeHash();
            }

            this._allSizeHashesKnown = true;
        }

        private void ComputeQuickHashes()
        {
            foreach (var file in _fileList.Where(f => f.QuickHashKnown == false))
            {
                file.ComputeQuickHash();
            }

            this._allQuickHashesKnown = true;
        }

        private void ComputeFullHashes()
        {
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