using System;
using System.ComponentModel;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.UI
{
    public partial class FormMain : Form
    {

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();

        readonly FileSearcher _fileSearcher = new FileSearcher();

        public FormMain()
        {
            _log.Debug("Base Form constructing");
            InitializeComponent();
            _log.Debug("Base Form constructed");
        }

        private void DoSearching(object sender, EventArgs e)
        {
            _log.Info("CLICK button '{0}'", cbSearch.Name);

            _log.Trace("Initializing engine");
            DupeFinder finder = DupeFinder.Finder.Initialize(txtSearchPath.Text);

            _log.Trace("Creating Background workers");
            BackgroundWorker bgSearch = new BackgroundWorker();
            bgSearch.DoWork += Workers.WorkerSearchForFiles;

            BackgroundWorker bgHash = new BackgroundWorker();
            bgHash.DoWork += Workers.WorkerDoHashing;

            BackgroundWorker bgGroup = new BackgroundWorker();
            bgHash.DoWork += Workers.WorkerGroupFiles;

/*
            finder.SearchForFiles();
            finder.

            // TODO: cleanup this shitty datapath - single responsability !
            _log.Info("Start processing");
            _fileSearcher.Reset()
                         .SetDirectory(DupeFinder.Finder.SearchPath.FullName)
                         .BuildFileList(FileSearchOption.BgComputeHash);

            _fileSearcher.CompareHashes(HashingType.SizeHashing);
            _fileSearcher.CompareHashes(HashingType.QuickHashing);
            _fileSearcher.CompareHashes(HashingType.FullHashing);
 * */

            _log.Info("Finished button {0} routine", cbSearch.Name);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
