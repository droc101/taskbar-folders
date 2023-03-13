using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskbarFolders
{
    public partial class ExceptionHandler : Form
    {
        public ExceptionHandler(Exception ex)
        {
            InitializeComponent();
            label1.Text = ex.TargetSite.Module.Name + ": " + ex.Message;
        }

        private void aeroLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/droc101/taskbar-folders/issues/new");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
