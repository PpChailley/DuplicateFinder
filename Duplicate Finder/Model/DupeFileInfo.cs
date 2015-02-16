using System;
using System.IO;
using Gbd.Sandbox.DuplicateFinder.Forms.Model;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    class DupeFileInfo
    {

        FileInfo _fileInfo;

        SizeHash _sizeHash = null;
        QuickHash _quickHash = null;
        FullHash _fullHash = null;


        public DupeFileInfo(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;
        }

        public bool SizeHashKnown { get { return _sizeHash != null;  } }
        public bool QuickHashKnown { get { return _quickHash != null;  } }
        public bool FullHashKnown { get { return _fullHash != null;  } }



        public void ComputeSizeHash()
        {
            _sizeHash = new SizeHash(_fileInfo);
        }
        public void ComputeQuickHash()
        {
            _quickHash = new QuickHash(_fileInfo);
        }
        public void ComputeFullHash()
        {
            _fullHash = new FullHash(_fileInfo);
        }


    }
}
