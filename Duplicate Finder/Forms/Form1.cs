using System;
using System.IO;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;
using Gbd.Sandbox.DuplicateFinder.Model.Hashing;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Forms
{
    public partial class Form1 : Form
    {

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        readonly FileSearcher _fileSearcher = new FileSearcher();

        public Form1()
        {
            _log.Debug("Base Form constructing");
            InitializeComponent();
            _log.Debug("Base Form constructed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _log.Info("CLICK button '{0}'", button1.Name);

            DupeFinder.Finder.SearchPath = new DirectoryInfo(cbSearchPath.Text);

            // TODO: cleanup this shitty datapath - single responsability !
            _log.Info("Start processing");
            _fileSearcher.Reset()
                         .SetDirectory(DupeFinder.Finder.SearchPath.FullName)
                         .BuildFileList(FileSearchOption.BgComputeHash);

            _fileSearcher.CompareHashes(HashingType.SizeHashing);
            _fileSearcher.CompareHashes(HashingType.QuickHashing);
            _fileSearcher.CompareHashes(HashingType.FullHashing);

            _log.Info("Finished button {0} routine", button1.Name);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
