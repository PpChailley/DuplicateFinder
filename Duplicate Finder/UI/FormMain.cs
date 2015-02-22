using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.UI
{
    public partial class FormMain : Form
    {

        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        readonly FileSearcher _fileSearcher = new FileSearcher();

        public FormMain()
        {
            Log.Debug("Base Form constructing");
            InitializeComponent();
            Log.Debug("Base Form constructed");
        }

        private void DoSearching(object sender, EventArgs e)
        {
            Log.Info("CLICK button '{0}'", cbSearch.Name);

            Log.Trace("Initializing engine");
            DupeFinder finder = DupeFinder.Finder.Initialize(txtSearchPath.Text);

            Log.Trace("Creating Background workers");
            BackgroundWorker bgSearch = new BackgroundWorker();
            bgSearch.DoWork += Workers.WorkerSearchForFiles;

            BackgroundWorker bgHash = new BackgroundWorker();
            bgHash.DoWork += Workers.WorkerDoHashing;

            BackgroundWorker bgGroup = new BackgroundWorker();
            bgGroup.DoWork += Workers.WorkerGroupFiles;


            bgSearch.RunWorkerAsync();
            Log.Warn("*** SLEEPING TO WAIT FOR BG /SEARCH/ COMPLETION ***");
            Thread.Sleep(200);
            Log.Warn("*** WAKING FROM SLEEP ***");


            bgHash.RunWorkerAsync();
            Log.Warn("*** SLEEPING TO WAIT FOR BG /HASH/ COMPLETION ***");
            Thread.Sleep(200);
            Log.Warn("*** WAKING FROM SLEEP ***");

            bgGroup.RunWorkerAsync();
            Log.Warn("*** SLEEPING TO WAIT FOR BG /GROUP/ COMPLETION ***");
            Thread.Sleep(200);
            Log.Warn("*** WAKING FROM SLEEP ***");



/*
            finder.DoSearchForFiles();
            finder.

            // TODO: cleanup this shitty datapath - single responsability !
            Log.Info("Start processing");
            _fileSearcher.Reset()
                         .SetDirectory(DupeFinder.Finder.SearchPath.FullName)
                         .BuildFileList(FileSearchOption.BgComputeHash);

            _fileSearcher.CompareHashes(HashingType.SizeHashing);
            _fileSearcher.CompareHashes(HashingType.QuickHashing);
            _fileSearcher.CompareHashes(HashingType.FullHashing);
 * */

            Log.Info("Finished button {0} routine", cbSearch.Name);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
