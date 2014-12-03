using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace HappyPanel
{
    static class Program
    {
        static Process process;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain domain = AppDomain.CurrentDomain;
            domain.DomainUnload += domain_DomainUnload;

            string filename = domain.GetData("filename") as string;
            string argument = domain.GetData("argument") as string;
            if (!string.IsNullOrEmpty(filename))
            {
                string message = "";
                try
                {
                    process = Process.Start(filename, argument);
                    process.WaitForExit();
                    message = "process exited";
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                domain.SetData("message", message);
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }

        static void domain_DomainUnload(object sender, EventArgs e)
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
}
