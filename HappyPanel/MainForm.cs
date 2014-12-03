using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HappyPanel.Properties;
using System.Diagnostics;

namespace HappyPanel
{
    public partial class MainForm : Form
    {
        Process process;
        bool restartSignal;

        class Info
        {
            public Info(string filename, string argument)
            {
                this.filename = filename;
                this.argument = argument;
            }

            public string filename;
            public string argument;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        void log(string text)
        {
            logRichTextBox.AppendText(text + "\n");
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            this.Opacity = 0.5;
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {
            //this.Opacity = 1;
        }

        private void MainForm_MouseLeave(object sender, EventArgs e)
        {
            //this.Opacity = 0.5;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            start();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Info info = e.Argument as Info;

            try
            {
                process = Process.Start(info.filename, info.argument);
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
            finally
            {
                process = null;
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startButton.Enabled = true;
            stopButton.Enabled = false;
            restartButton.Enabled = false;

            Exception ex = e.Result as Exception;
            if (ex != null)
            {
                log(ex.Message);
            }
            else
            {
                log("process exited");
            }

            if (restartSignal)
            {
                restartSignal = false;
                start();
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stop();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            restart();
        }

        void start()
        {
            string file = fileTextBox.Text;
            if (string.IsNullOrWhiteSpace(file)) return;

            string arg = argTextBox.Text;

            log("[start] " + file + " " + arg);

            startButton.Enabled = false;
            stopButton.Enabled = true;
            restartButton.Enabled = true;

            backgroundWorker.RunWorkerAsync(new Info(file, arg));
        }

        void stop()
        {
            log("[stop]");

            if (process == null) return;
            try
            {
                process.Kill();
            }
            catch (Exception ex)
            {
                log(ex.Message);
            }
        }

        void restart()
        {
            log("[restart]");

            startButton.Enabled = false;
            stopButton.Enabled = false;
            restartButton.Enabled = false;

            stop();
            restartSignal = true;
        }

        private void MainForm_Enter(object sender, EventArgs e)
        {

        }

        private void MainForm_Leave(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Rectangle rect = Screen.PrimaryScreen.WorkingArea;
            Size size = this.Size;
            this.Location = new Point(rect.Width - size.Width - 10, rect.Height - size.Height - 10);
        }
    }
}
