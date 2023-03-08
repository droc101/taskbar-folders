using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace TaskbarFolders
{
    public partial class Form1 : Form
    {
        bool fr = true;

        public Form1()
        {
            InitializeComponent();
            AddPinnedItem(@"C:\Windows\system32\cmd.exe");
            AddPinnedItem(@"C:\Windows\system32\notepad.exe");
            AddPinnedItem(@"C:\Windows\system32\taskmgr.exe");
        }

        Program.Settings settings;
        int settingsIndex;

        public Form1(Program.Settings settings, int settingsIndex)
        {
            InitializeComponent();
            
            Round.RoundWindow(this);
            foreach (string pin in settings.Pins)
            {
                AddPinnedItem(pin);
            }
            this.settings = settings;
            this.settingsIndex = settingsIndex;
            UpdateLooks();
            Size = new Size(350, 222);
            
        }

        void UpdateLooks()
        {
            notifyIcon1.Text = settings.Name;
            if (settings.useColor)
            {
                Bitmap tint = IconUtils.ApplyTint(Properties.Resources.generics_folder_png, settings.color, 0.5f);
                notifyIcon1.Icon = Icon.FromHandle(tint.GetHicon());
            }
            else
            {
                notifyIcon1.Icon = Icon.FromHandle(new Bitmap(settings.ImagePath).GetHicon());
            }
        }

        string GetItemDesc(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                return new DirectoryInfo(filePath).Name;
            }
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);
            string desc = myFileVersionInfo.FileDescription;
            if (String.IsNullOrEmpty(desc))
            {
                desc = new FileInfo(filePath).Name;
            }
            return desc;
        }

        public void AddPinnedItem(string filePath)
        {
            string desc = GetItemDesc(filePath);
            ListViewItem lvi = new ListViewItem();
            lvi.Text = desc;
            Image bmp = IconUtils.GetLargestIcon(filePath);
            imageList1.Images.Add(filePath, bmp);
            lvi.ImageKey = filePath;
            if (filePath.EndsWith(".exe"))
            {
                lvi.Group = aeroListView1.Groups[1];
            } else
            {
                lvi.Group = aeroListView1.Groups[0];
            }
            aeroListView1.Items.Add(lvi);
        }

        private void aeroListView1_ItemActivate(object sender, EventArgs e)
        {
            Process.Start(aeroListView1.SelectedItems[0].ImageKey);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (fr)
            {
                fr = false;
                Hide();
            }
            cueTextBox1.ResetText();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        void SavePins()
        {
            Program.currentSettings[settingsIndex] = settings;
            Program.SaveSettings();
        }

        private void addItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (settings.Pins.Contains(openFileDialog1.FileName))
                {
                    MessageBox.Show("This item is already pinned.", "Can't pin", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                AddPinnedItem(openFileDialog1.FileName);
                settings.Pins.Add(openFileDialog1.FileName);
                SavePins();
            }
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            if (aeroListView1.SelectedItems.Count > 0)
            {
                removeItemToolStripMenuItem.Enabled= true;
            } else
            {
                removeItemToolStripMenuItem.Enabled= false;
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button== MouseButtons.Left)
            {
                Location = Point.Subtract((Point)Screen.PrimaryScreen.WorkingArea.Size, Size);
                Location = Point.Subtract(Location, new Size(10, 10));
                cueTextBox1.ResetText();
                aeroListView1.Focus();
                Show();
                Activate();
            }
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pinIndex = settings.Pins.IndexOf(openFileDialog1.FileName)+1;
            settings.Pins.RemoveAt(pinIndex);
            aeroListView1.Items.RemoveAt(pinIndex);
            SavePins();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void programSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFolderDialog efd = new EditFolderDialog(settings);
            if (efd.ShowDialog() == DialogResult.OK)
            {
                settings = efd.settings;
                SavePins();
                UpdateLooks();
            }
        }

        private void deleteFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Program.currentSettings.Count == 1)
            {
                MessageBox.Show("You can't delete the only folder.", "Can't delete", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            } else
            {
                Program.currentSettings.RemoveAt(settingsIndex);
                Program.SaveSettings();
                Close();
            }
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings newf = new Program.Settings();
            newf.Name = "New Folder";
            newf.Pins = new List<string>();
            Program.currentSettings.Add(newf);
            int idx = Program.currentSettings.IndexOf(newf);
            Program.currentSettings.Remove(newf);
            EditFolderDialog efd = new EditFolderDialog(newf);
            if (efd.ShowDialog() == DialogResult.OK)
            {
                Program.currentSettings.Add(efd.settings);
                new Form1(efd.settings, idx).Show();
                Program.SaveSettings();
            }
        }

        private void cueTextBox1_TextChanged(object sender, EventArgs e)
        {
            aeroListView1.Items.Clear();
            foreach (string pin in settings.Pins)
            {
                if (GetItemDesc(pin).ToLower().Contains(cueTextBox1.Text.ToLower()) || String.IsNullOrEmpty(cueTextBox1.Text))
                {
                    AddPinnedItem(pin);
                }
            }
            if (aeroListView1.Items.Count > 0)
            {
                aeroListView1.SelectedIndices.Clear();
                aeroListView1.SelectedIndices.Add(0);
            }
        }

        private void cueTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                if (aeroListView1.Items.Count > 0)
                {
                    Process.Start(aeroListView1.SelectedItems[0].ImageKey);
                }
            } else if (e.KeyCode == Keys.Down)
            {
                aeroListView1.Focus();
            }
        }

        private void aeroListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (aeroListView1.SelectedIndices.Count == 0)
                {
                    cueTextBox1.Focus();
                }
                else if (aeroListView1.SelectedIndices[0] < 3)
                {
                    cueTextBox1.Focus();
                }
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (settings.Pins.Contains(openFileDialog1.FileName))
                {
                    MessageBox.Show("This item is already pinned.", "Can't pin", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                AddPinnedItem(folderBrowserDialog1.SelectedPath);
                settings.Pins.Add(folderBrowserDialog1.SelectedPath);
                SavePins();
            }
        }
    }
}
