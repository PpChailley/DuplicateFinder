using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class DupeFinder
    {

        public static DupeFinder Finder  = new DupeFinder();


        public DirectoryInfo SearchPath;
        public SearchOptions SearchOptions;


    }
}
