using System;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;

namespace Gbd.Sandbox.DuplicateFinder.Forms.Forms
{
    public partial class Form1 : Form
    {

        DupeFinder _dupeFinder = new DupeFinder();
        FileSearcher _fileSearcher = new FileSearcher();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _fileSearcher.Reset()
                         .SetDirectory(this.comboBox1.Text)
                         .BuildFileList();

            
            
        }
    }
}
