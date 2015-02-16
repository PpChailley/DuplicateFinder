using System;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Forms.Forms
{
    public partial class Form1 : Form
    {

        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        DupeFinder _dupeFinder = new DupeFinder();
        FileSearcher _fileSearcher = new FileSearcher();

        public Form1()
        {
            _log.Debug("Base Form constructing");
            InitializeComponent();
            _log.Debug("Base Form constructed");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _log.Info("CLICK button '{0}'", button1.Name);

            _log.Info("Start processing");
            _fileSearcher.Reset()
                         .SetDirectory(this.comboBox1.Text)
                         .BuildFileList(FileSearchOption.BgComputeHash);

            _log.Info("Finished button {0} routine", button1.Name);
            
        }
    }
}
