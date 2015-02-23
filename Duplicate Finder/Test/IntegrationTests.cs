using System;
using System.Collections.Generic;
using System.Linq;
using Gbd.Sandbox.DuplicateFinder.Model;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{
    [TestFixture]
    public class IntegrationTests : TestsBase
    {


        [Test]
        public void SmokeTest()
        {
            Finder.HashingSequence = new[] {HashingType.QuickHashing, HashingType.FullHashing,};
            Finder.DoSearchForFiles();
            Finder.DoHashing();
            Finder.DoCompareHashedResults();

            Assert.That(Finder.Similars.Count, Is.EqualTo(17));
            Assert.That(Finder.Similars.Depth, Is.EqualTo(2));

            Assert.That(Finder.Similars.Map, Is.EqualTo(null));
            Assert.That(Finder.Similars.HashingType, Is.EqualTo(HashingType.SizeHashing));
            Assert.That(Finder.Similars.RefinedMaps.Count(), Is.EqualTo(4));
        }

 


    }
}
