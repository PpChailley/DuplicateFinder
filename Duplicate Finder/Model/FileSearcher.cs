using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class FileSearcher
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    

        internal DirectoryInfo BaseDirectory;
        internal SearchOptions Options = new SearchOptions();

        //public ICollection<DupeFileInfo> FileList;
        public BlockingCollection<DupeFileInfo> FileList;


        public FileSearcher()
        {
            Initialize();
        }





        private void Initialize()
        {
            BaseDirectory = null;
            FileList = new BlockingCollection<DupeFileInfo>();
        }


        public FileSearcher Reset()
        {
            Initialize();
            Log.Info("Reset of FileSearcher successful");
            return this;
        }


        public FileSearcher SetDirectory(DirectoryInfo newDirectory)
        {
            Reset();
            BaseDirectory = newDirectory;

            Log.Info("FileSearcher now working in directory '{0}'", BaseDirectory.FullName);

            return this;
        }

        public FileSearcher BuildFileList()
        {
            Log.Info("Start building file list");

            foreach (var curFile in 
                BaseDirectory.GetFiles("*", SearchOption.AllDirectories)
                    .Where(file => file.Exists)
                    .Where(file => Options.Matches(file)))
            {
                var info = new DupeFileInfo(curFile);
                FileList.Add(info);
            }

            Log.Info("Found {0} files in search directory", FileList.Count);



            return this;
        }


        public void SetDirectory(string newDirectory)
        {
            SetDirectory(new DirectoryInfo(newDirectory));
        }

        public void SetOptions(SearchOptions.Flag flags)
        {
            this.Options.Flags = flags;
        }

        
    }
}