﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gbd.Sandbox.DuplicateFinder.Forms.Forms;
using NLog;
using NLog.Config;

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
            LogManager.Configuration = new XmlLoggingConfiguration(@"NLog.xml");

            AssertThatNLogIsConfigured();


            _log.Info("Application starting");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static void AssertThatNLogIsConfigured()
        {
            var targets = LogManager.Configuration.AllTargets;

            if (targets.Any(x => x.Name.Equals("AssertTarget")) == false)
            {
                throw new Exception("NLog is not configured !!");
            }
        }
    }

    // TODO: This is a git placeholder


}
