using System;
using System.Threading;
using Gbd.Sandbox.DuplicateFinder.Model;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{
    public abstract class TestsBase
    {
        protected const String TEST_DIR_PATH = @"S:\Dropbox\Visual Studio\Sandboxes\Duplicate Finder\TestDataSet";
        protected DupeFinder Finder;

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
            Finder = new DupeFinder();
            Finder.Initialize(TEST_DIR_PATH);

            Initialize();
        }

        [TearDown]
        public void TestTearDown()
        {
            Thread.Sleep(0);
        }

        protected virtual void Initialize() {}


    }
}