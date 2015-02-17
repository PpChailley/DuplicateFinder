using System;
using System.IO;
using Gbd.Sandbox.DuplicateFinder.Model;

namespace Gbd.Sandbox.DuplicateFinder.UI
{
    public class DuplicateRow
    {
        public String Name;
        public String FilePath;
        public String Size;
        public bool Checked;
        public int GroupId;
        public String WastedSpace;

        private DuplicateRow(DupeFileInfo file)
        {
            Name = file.FileInfo.Name;
            FilePath = GetRelativePath(file.FileInfo.DirectoryName, DupeFinder.Finder.SearchPath.FullName);
            Size = MakeHumanReadableNumber(file.FileInfo.Length);
            Checked = false;
        }

      // TODO  public DuplicateRow[] MakeRowsFromDuplicateGroup(grou)

        // TODO: Move to lib
        public string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        // TODO: Move to lib
        public string MakeHumanReadableNumber(double len)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }

            string result = String.Format("{0:0.#} {1}", len, sizes[order]);

            return result;
        }
    }
}