using System.Threading;
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
}