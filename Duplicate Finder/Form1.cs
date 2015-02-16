using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Model;


namespace Gbd.Sandbox.DuplicateFinder.Forms
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
