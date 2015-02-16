using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Forms.Forms;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Forms
{
    static class Program
    {
        private static readonly Logger _log = LogManager.GetCurrentClassLogger();    

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            _log.Info("Application starting");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    // TODO: This is a git placeholder
    // PLACEHOLDER


}
