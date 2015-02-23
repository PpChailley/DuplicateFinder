using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gbd.Sandbox.DuplicateFinder.Model;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{
    [TestFixture]
    public class IntegrationTests 
    {

        internal class DupeFinderBoxOpener : DupeFinder
        {

            internal SimilarityMapBoxOpener OpenSimilarityMap
            {
                get { return  new SimilarityMapBoxOpener(Similars); }
            }

        }

        internal class SimilarityMapBoxOpener : SimilarityMap
        {
            public HashSet<FileEquivalenceClass> OpenMap    { get { return Map; }}
            public IEnumerable<SimilarityMapBoxOpener> OpenRefined  {get { return OpenBox(RefinedMaps); }}

            private static IEnumerable<SimilarityMapBoxOpener> OpenBox(IEnumerable<SimilarityMap> boxesToOpen)
            {
                return boxesToOpen.Cast<SimilarityMapBoxOpener>();
            }


            public SimilarityMapBoxOpener(IEnumerable<DupeFileInfo> files, HashingType hashingType) : base(files, hashingType) { }

            public SimilarityMapBoxOpener(SimilarityMap similars) : base()
            {
                
            }
        }


        private DupeFinderBoxOpener _finder;

        private const String TEST_DIR_PATH = @"S:\Dropbox\Visual Studio\Sandboxes\Duplicate Finder\TestDataSet";



        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Thread.Sleep(0);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Thread.Sleep(0);
        }


        [SetUp]
        public void Initialize()
        {
            _finder = new DupeFinderBoxOpener();
            _finder.Initialize(TEST_DIR_PATH);
        }

        [TearDown]
        public void TestTearDown()
        {
            
        }



        [Test]
        public void SmokeTest()
        {
            _finder.HashingSequence = new[] {HashingType.QuickHashing, HashingType.FullHashing,};
            _finder.DoSearchForFiles();
            _finder.DoHashing();
            _finder.DoCompareHashedResults();

            Assert.That(_finder.OpenSimilarityMap.Count, Is.EqualTo(17));
            Assert.That(_finder.OpenSimilarityMap.Depth, Is.EqualTo(2));
            
            Assert.That(_finder.OpenSimilarityMap.OpenMap, Is.EqualTo(null));
            Assert.That(_finder.OpenSimilarityMap.HashingType, Is.EqualTo(HashingType.SizeHashing));
            Assert.That(_finder.OpenSimilarityMap.OpenRefined.Count(), Is.EqualTo(4));




        }





    }

}
