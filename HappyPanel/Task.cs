using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;

namespace HappyPanel
{
    public class Task : BackgroundWorker
    {
        public Task(string filename, string argument)
        {
            this.filename = filename;
            this.argument = argument;
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            base.OnDoWork(e);

            try
            {
                domain = AppDomain.CreateDomain("background");
                domain.SetData("filename", filename);
                domain.SetData("argument", argument);
                domain.ExecuteAssemblyByName(Assembly.GetExecutingAssembly().FullName);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnProgressChanged(ProgressChangedEventArgs e)
        {
            base.OnProgressChanged(e);
        }

        protected override void OnRunWorkerCompleted(RunWorkerCompletedEventArgs e)
        {
            base.OnRunWorkerCompleted(e);
        }

        public void Abort()
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);
            }
        }

        string filename;
        string argument;
        AppDomain domain;
    }
}
