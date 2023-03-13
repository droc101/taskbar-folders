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
            Round.MicaWindow(this);
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
            foreach (Extension ex in Program.extensions)
            {
                Extension.ExtensionInfo info = ex.GetInfo();
                ListViewItem lvi = new ListViewItem();
                lvi.Text = info.Name;
                lvi.SubItems.Add(info.Description);
                lvi.SubItems.Add(info.Developer);
                lvi.SubItems.Add(info.Version.ToString());
                aeroListView2.Items.Add(lvi);
            }
            if (aeroListView2.Items.Count > 0)
            {
                aeroListView2.Items[0].Selected = true;
            }
            foreach (Extension ex in Program.extensions)
            {
                TabPage exp = ex.SettingsHandler();
                if (exp != null)
                {
                    tabControl1.TabPages.Add(exp);
                }
            }
            checkBox1.Checked = Program.currentSettings.ShowFileExtensions;
            checkBox2.Checked = Program.currentSettings.AlwaysShowWelcome;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.currentSettings.ShowFileExtensions = checkBox1.Checked;
            Program.currentSettings.AlwaysShowWelcome = checkBox2.Checked;
            Program.SaveSettings();
            DialogResult= DialogResult.OK;
        }

        private void aeroListView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (aeroListView2.SelectedItems.Count > 0)
            {
                ListViewItem plugin_info = aeroListView2.SelectedItems[0];
                captionLabel1.Text = plugin_info.Text;
                label5.Text = "Version " + plugin_info.SubItems[3].Text + " by " + plugin_info.SubItems[2].Text;
                textBox1.Text = plugin_info.SubItems[1].Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(Program.PluginsPath);
        }
    }
}
