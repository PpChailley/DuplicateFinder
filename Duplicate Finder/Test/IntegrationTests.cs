﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gbd.Sandbox.DuplicateFinder.Model;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{
    public abstract class TestsBase
    {
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
        public void SetUp()
        {
            Initialize();
        }

        [TearDown]
        public void TestTearDown()
        {
            Thread.Sleep(0);
        }

        protected abstract void Initialize();


    }

    [TestFixture]
    public class IntegrationTests : TestsBase
    {

        private readonly DupeFinder _finder = new DupeFinder();
        private const String TEST_DIR_PATH = @"S:\Dropbox\Visual Studio\Sandboxes\Duplicate Finder\TestDataSet";

        protected override void Initialize()
        {
            _finder.Initialize(TEST_DIR_PATH);
        }




        [Test]
        public void SmokeTest()
        {
            _finder.HashingSequence = new[] {HashingType.QuickHashing, HashingType.FullHashing,};
            _finder.DoSearchForFiles();
            _finder.DoHashing();
            _finder.DoCompareHashedResults();

            Assert.That(_finder.Similars.Count, Is.EqualTo(17));
            Assert.That(_finder.Similars.Depth, Is.EqualTo(2));

            Assert.That(_finder.Similars.Map, Is.EqualTo(null));
            Assert.That(_finder.Similars.HashingType, Is.EqualTo(HashingType.SizeHashing));
            Assert.That(_finder.Similars.RefinedMaps.Count(), Is.EqualTo(4));
        }


    }

}
