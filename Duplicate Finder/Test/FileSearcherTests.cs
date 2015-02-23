using System;
using System.IO;
using Gbd.Sandbox.DuplicateFinder.Model;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{

    [TestFixture]
    public class FileSearcherTests: TestsBase
    {

        private const string INVALID_PATH = "An invalid path";

        public FileSearcher Searcher;

        



        protected override void Initialize()
        {
            base.Initialize();
            Searcher = new FileSearcher();
        }



        [Test]
        public void TestReset()
        {
            Searcher.SetDirectory(TEST_DIR_PATH);
            Searcher.Reset();

            Assert.That(Searcher.BaseDirectory, Is.EqualTo(null));
        }

        [Test]
        public void TestSetDirectory()
        {
            Searcher.SetDirectory(TEST_DIR_PATH);
            
            Assert.That(Searcher.BaseDirectory, Is.EqualTo(new DirectoryInfo(TEST_DIR_PATH)));
        }

        [TestCase(TEST_DIR_PATH)]
        [TestCase("An Invalid path")]
        public void TestSetDirectoryValidity(string dirpath)
        {
            Searcher.SetDirectory(dirpath);

            Assert.That(Searcher.BaseDirectory, Is.EqualTo(new DirectoryInfo(dirpath)));
        }

        [Test]
        public void TestEmptyFilelist()
        {
            Searcher.SetDirectory(TEST_DIR_PATH);

            Assert.That(Searcher.FileList, Is.Empty);
        }

        [Test]
        public void TestReassignPath()
        {
            
            Searcher.SetDirectory(TEST_DIR_PATH);
            Searcher.SetDirectory(INVALID_PATH);

            Assert.That(Searcher.BaseDirectory, Is.EqualTo(new DirectoryInfo(INVALID_PATH)));
        }


        [TestCase(TEST_DIR_PATH, TEST_DIR_PATH)]
        [TestCase(TEST_DIR_PATH, INVALID_PATH)]
        public void TestFileListIsResetWhenPathChanges(string path0, string path1)
        {
            Searcher.SetDirectory(path0);
            Searcher.BuildFileList();
            Searcher.SetDirectory(path1);

            Assert.That(Searcher.FileList, Is.Empty);
        }

        [TestCase(TEST_DIR_PATH, SearchOptions.Flag.IncludeAll, 17)]
        [TestCase(TEST_DIR_PATH, SearchOptions.Flag.NoHiddenFiles, 15)]
        [TestCase(TEST_DIR_PATH, SearchOptions.Flag.NoArchiveFiles, 15)]
        public void TestFileSearch(string path, SearchOptions.Flag options, int expectedNbFiles)
        {
            Searcher.SetDirectory(path);
            Searcher.SetOptions(options);
            Searcher.BuildFileList();

            Assert.That(Searcher.FileList.Count, Is.EqualTo(expectedNbFiles));
        }







    }
}