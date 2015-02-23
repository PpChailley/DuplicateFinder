using System;
using System.IO;
using System.Linq;
using Gbd.Sandbox.DuplicateFinder.Model;
using NUnit.Framework;

namespace Gbd.Sandbox.DuplicateFinder.Test
{

    [TestFixture]
    public class SearchOptionsTests: TestsBase
    {
        private SearchOptions _options;

        protected override void Initialize()
        {
            _options = new SearchOptions();
        }

        private static FileInfo GetAnyFile(FileAttributes attributes, DirectoryInfo dir = null)
        {
            if (dir == null)
                dir = new DirectoryInfo(Environment.SystemDirectory).Root;

            try
            {
                var found = dir.GetFiles().FirstOrDefault(file => file.Attributes.Equals(attributes));
                if (found != null)
                    return found;
            }
            catch (UnauthorizedAccessException) { }

            try
            {
                foreach (var subDir in dir.GetDirectories())
                {
                    var foundRecursive = GetAnyFile(attributes, subDir);
                    if (foundRecursive != null)
                        return foundRecursive;
                }
            }
            catch (UnauthorizedAccessException) { }

            return null;
        }


        [TestCase(FileAttributes.Normal, SearchOptions.Flag.IncludeAll, true)]
        [TestCase(FileAttributes.Normal, SearchOptions.Flag.NoHiddenFiles, true)]
        [TestCase(FileAttributes.Normal, SearchOptions.Flag.NoArchiveFiles, true)]
        [TestCase(FileAttributes.Normal, SearchOptions.Flag.NoSystemFiles, true)]
        [TestCase(FileAttributes.Hidden, SearchOptions.Flag.NoSystemFiles, true)]
        [TestCase(FileAttributes.Hidden, SearchOptions.Flag.NoHiddenFiles, false)]
        //[TestCase(FileAttributes.System, SearchOptions.Flag.NoHiddenFiles, true)]
        //[TestCase(FileAttributes.System, SearchOptions.Flag.NoSystemFiles, false)]
        public void FileMatcherTest(
            FileAttributes attributes, 
            SearchOptions.Flag searchOptions, 
            bool shouldMatch)
        {
            var testFile = GetAnyFile(attributes);
            if (testFile == null)
                throw new InconclusiveException("Cound not find a suitable test file");

            _options.Flags = searchOptions;
            var matcherResult = _options.Matches(testFile);

            Assert.That(matcherResult, Is.EqualTo(shouldMatch));
        }

     
    }
}