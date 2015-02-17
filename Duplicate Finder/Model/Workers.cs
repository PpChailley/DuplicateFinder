using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace Gbd.Sandbox.DuplicateFinder.Model
{
    public static class Workers
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();    


        public static void WorkerSearchForFiles(object sender, DoWorkEventArgs e)
        {
            if (Thread.CurrentThread.Name != null)
                throw new InvalidAsynchronousStateException("This routine should not be called outside its own BG thread");

            Thread.CurrentThread.Name = "WorkerSearchForFiles";
            Log.Info("Start BG routine WorkerSearchForFiles");

            DupeFinder.Finder.SearchForFiles();
        }


        public static void WorkerDoHashing(object sender, DoWorkEventArgs e)
        {
            if (Thread.CurrentThread.Name != null)
                throw new InvalidAsynchronousStateException("This routine should not be called outside its own BG thread");

            Thread.CurrentThread.Name = "WorkerDoHashing";
            Log.Info("Start BG routine WorkerDoHashing");

            DupeFinder.Finder.DoHashing();
        }


        public static void WorkerGroupFiles(object sender, DoWorkEventArgs e)
        {
            if (Thread.CurrentThread.Name != null)
                throw new InvalidAsynchronousStateException("This routine should not be called outside its own BG thread");

            Thread.CurrentThread.Name = "WorkerGroupFiles";
            Log.Info("Start BG routine WorkerGroupFiles");

            DupeFinder.Finder.DoCompareHashedResults();
        }
    }
}
