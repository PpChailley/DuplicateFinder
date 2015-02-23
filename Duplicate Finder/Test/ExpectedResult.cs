using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gbd.Sandbox.DuplicateFinder.Model;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;

namespace Gbd.Sandbox.DuplicateFinder.Test
{
    public class ExpectedResult : List<KeyValuePair<HashingType, ComparisonResult>>
    {

        public void Compare(SimilarityMap mapToCompare)
        {
            
        }
    }

    public class ComparisonResult
    {
        public HashSet<TestEquivalentFileSet> Data;
    }

    public class TestEquivalentFileSet
    {
        public String FilenameOfAnyFile;
        public int GroupSize;
    }
}
