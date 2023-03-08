using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskbarFolders
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void aeroLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/droc101/taskbar-folders");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void aeroLinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://droc101.dev");
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            label4.Text = Application.ProductVersion.ToString();
        }
    }
}
