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
using System.Reflection;

namespace HappyPanel
{
    public partial class MainForm : Form
    {
        Task task;
        bool restartSignal = false;

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
            string filename = fileTextBox.Text;
            if (string.IsNullOrWhiteSpace(filename)) return;

            string argument = argTextBox.Text;

            log("[start] " + filename + " " + argument);

            startButton.Enabled = false;
            stopButton.Enabled = true;
            restartButton.Enabled = true;

            task = new Task(filename, argument);
            task.RunWorkerCompleted += task_RunWorkerCompleted;
            task.RunWorkerAsync();
        }

        void task_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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
            else
            {
                startButton.Enabled = true;
                stopButton.Enabled = false;
                restartButton.Enabled = false;
            }
        }

        void stop()
        {
            log("[stop]");

            startButton.Enabled = false;
            stopButton.Enabled = false;
            restartButton.Enabled = false;

            task.Abort();
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
