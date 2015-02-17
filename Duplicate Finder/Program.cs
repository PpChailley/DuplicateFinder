using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.UI;
using NLog;
using NLog.Config;

namespace Gbd.Sandbox.DuplicateFinder
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
            LogManager.Configuration = new XmlLoggingConfiguration(@"NLog.xml");

            AssertThatNLogIsConfigured();


            _log.Info("Application starting");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }

        private static void AssertThatNLogIsConfigured()
        {
            var targets = LogManager.Configuration.AllTargets;

            if (targets.Any(x => x.Name.Equals("AssertTarget")) == false)
            {
                throw new Exception("NLog is not configured !!");
            }

            _log.Info("Logger seems to work OK");
        }
    }

    // PLACEHOLDER
    // Placeholder

}
