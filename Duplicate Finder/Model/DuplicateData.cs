using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gbd.Sandbox.DuplicateFinder.Forms;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public class DuplicateData
    {


    }


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


        string GetRelativePath(string filespec, string folder)
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


        private string MakeHumanReadableNumber(double len)
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
