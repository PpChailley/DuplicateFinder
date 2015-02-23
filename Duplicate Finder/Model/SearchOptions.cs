using System;
using System.IO;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class SearchOptions
    {
        [Flags]
        public enum Flag : int
        {
            IncludeAll = 0,
            NoHiddenFiles = 1,
            NoSystemFiles = 2,
            NoArchiveFiles = 4
        }

        public Flag Flags = Flag.IncludeAll;
        public string[] IgnoreMasks = new string[] { };
        public string[] OnlyMasks = new string[] { };



        public SearchOptions(Flag flags = Flag.IncludeAll)
        {
            Flags = flags;
        }

        public static SearchOptions IgnoreMatching(String mask)
        {
            return new SearchOptions() { IgnoreMasks = new[] { mask } };
        }

        public static SearchOptions OnlyMatching(String mask)
        {
            return new SearchOptions(){ OnlyMasks = new []{mask}};
        }


        public bool Matches(FileInfo file)
        {
            if ((Flags = Flags & Flag.NoHiddenFiles) > 0)
            {
                if (file.Attributes.HasFlag(FileAttributes.Hidden))
                    return false;
            }

            if ((Flags = Flags & Flag.NoArchiveFiles) > 0)
            {
                if (file.Attributes.HasFlag(FileAttributes.Archive))
                    return false;
            }

            if ((Flags = Flags & Flag.NoSystemFiles) > 0)
            {
                if (file.Attributes.HasFlag(FileAttributes.System))
                    return false;
            }

            if (Flags != Flag.IncludeAll)
                throw new NotImplementedException(String.Format("This flag is unknown: 0x{0:X}", Flags));

            return true;
        }
    }



}