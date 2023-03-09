using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;

namespace TaskbarFolders
{
    public partial class Form1 : Form
    {
        bool fr = true;

        public Form1()
        {
            InitializeComponent();
        }

        List<ToolStripItem> folderContextItems;
        List<ToolStripItem> fileContextItems;
        List<ToolStripItem> mainContextItems;
        List<ToolStripItem> itemContextItems;

        Program.Folder settings;
        int settingsIndex;

        public Form1(Program.Folder settings, int settingsIndex)
        {
            InitializeComponent();
            
            Round.RoundWindow(this);
            foreach (Program.Pin pin in settings.Pins)
            {
                AddPinnedItem(pin);
            }
            this.settings = settings;
            this.settingsIndex = settingsIndex;
            UpdateLooks();
            Size = new Size(350, 222);
            fileContextItems = TransferMenuItems(fileContextMenu.Items);
            folderContextItems = TransferMenuItems(folderContextMenu.Items);
            mainContextItems= TransferMenuItems(mainContextMenu.Items);
            itemContextItems = TransferMenuItems(itemContextMenu.Items);
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

        string GetItemDesc(Program.Pin pin)
        {
            if (Directory.Exists(pin.Path))
            {
                return new DirectoryInfo(pin.Path).Name;
            }
            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(pin.Path);
            string desc = myFileVersionInfo.FileDescription;
            if (String.IsNullOrEmpty(desc))
            {
                desc = new FileInfo(pin.Path).Name;
            }
            return desc;
        }

        public void AddPinnedItem(string Path)
        {
            Program.Pin pin = new Program.Pin();
            pin.Path = Path;
            pin.Tags = new List<string>();
            AddPinnedItem(pin);
        }

        public void AddPinnedItem(Program.Pin pin)
        {
            string filePath = pin.Path;
            string desc = GetItemDesc(pin);
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
            Program.currentSettings.Folders[settingsIndex] = settings;
            Program.SaveSettings();
        }

        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
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

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void cueTextBox1_TextChanged(object sender, EventArgs e)
        {
            aeroListView1.Items.Clear();
            foreach (Program.Pin pin in settings.Pins)
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

        private void exitProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        void TransferMenuItems(ToolStripItemCollection from, ToolStripItemCollection to)
        {
            List<ToolStripItem> tempList = new List<ToolStripItem>();

            foreach (ToolStripItem item in from)
            {
                tempList.Add(item);
            }

            foreach (ToolStripItem item in tempList)
            {
                item.Tag = from;
                to.Add(item);
            }
        }

        void TransferMenuItems(List<ToolStripItem> from, ToolStripItemCollection to)
        {
            List<ToolStripItem> tempList = new List<ToolStripItem>();

            foreach (ToolStripItem item in from)
            {
                tempList.Add(item);
            }

            foreach (ToolStripItem item in tempList)
            {
                item.Tag = from;
                to.Add(item);
            }
        }

        List<ToolStripItem> TransferMenuItems(ToolStripItemCollection from)
        {
            List<ToolStripItem> tempList = new List<ToolStripItem>();

            foreach (ToolStripItem item in from)
            {
                tempList.Add(item);
            }

            foreach (ToolStripItem item in tempList)
            {
                item.Tag = from;
            }
            return tempList;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            contextMenuStrip1.Items.Clear();
            if (aeroListView1.SelectedItems.Count != 0)
            {
                if (Directory.Exists(aeroListView1.SelectedItems[0].ImageKey))
                {
                    TransferMenuItems(folderContextItems, contextMenuStrip1.Items);
                }
                else
                {
                    TransferMenuItems(fileContextItems, contextMenuStrip1.Items);
                }
                TransferMenuItems(itemContextItems, contextMenuStrip1.Items);
            }
            TransferMenuItems(mainContextItems, contextMenuStrip1.Items);
        }

        private void contextMenuStrip1_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (IsPinned(openFileDialog1.FileName))
                {
                    MessageBox.Show("This item is already pinned.", "Can't pin", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                AddPinnedItem(openFileDialog1.FileName);
                settings.Pins.Add(CreateDefaultPin(openFileDialog1.FileName));
                SavePins();
            }
        }

        Program.Pin CreateDefaultPin(string path)
        {
            Program.Pin pin = new Program.Pin();
            pin.Path = path;
            pin.Tags = new List<string>();
            return pin;
        }

        bool IsPinned(string path)
        {
            foreach (Program.Pin pin in settings.Pins)
            {
                if (pin.Path == path) return true;
            }
            return false;
        }

        private void addFolderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (IsPinned(folderBrowserDialog1.SelectedPath))
                {
                    MessageBox.Show("This item is already pinned.", "Can't pin", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                AddPinnedItem(folderBrowserDialog1.SelectedPath);
                settings.Pins.Add(CreateDefaultPin(folderBrowserDialog1.SelectedPath));
                SavePins();
            }
        }

        private void editFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditFolderDialog efd = new EditFolderDialog(settings);
            if (efd.ShowDialog() == DialogResult.OK)
            {
                settings = efd.settings;
                SavePins();
                UpdateLooks();
            }
        }

        private void addTaskbarFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Folder newf = new Program.Folder();
            newf.Name = "New Folder";
            newf.Pins = new List<Program.Pin>();
            newf.ImagePath = "";
            newf.color = Color.FromArgb(0, 120, 212);
            newf.useColor = true;
            Program.currentSettings.Folders.Add(newf);
            int idx = Program.currentSettings.Folders.IndexOf(newf);
            Program.currentSettings.Folders.Remove(newf);
            EditFolderDialog efd = new EditFolderDialog(newf);
            if (efd.ShowDialog() == DialogResult.OK)
            {
                Program.currentSettings.Folders.Add(efd.settings);
                new Form1(efd.settings, idx).Show();
                Program.SaveSettings();
            }
        }

        private void deleteTaskbarFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.currentSettings.Folders.Count == 1)
            {
                MessageBox.Show("You can't delete the only folder.", "Can't delete", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            else
            {
                Program.currentSettings.Folders.RemoveAt(settingsIndex);
                Program.SaveSettings();
                Close();
            }
        }

        private void exitProgramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SavePins();
            notifyIcon1.Visible= false;
            Process.GetCurrentProcess().Kill();
        }

        int FindPinIndex(string pin)
        {
            for (int i = 0; i < settings.Pins.Count; i++)
            {
                if (settings.Pins[i].Path == pin)
                {
                    return i;
                }
            }
            return -1;
        }

        private void removeItemToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int pinIndex = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
            settings.Pins.RemoveAt(pinIndex);
            aeroListView1.Items.RemoveAt(pinIndex);
            SavePins();
        }

        private void removeItemToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int pinIndex = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
            settings.Pins.RemoveAt(pinIndex);
            aeroListView1.Items.RemoveAt(pinIndex);
            SavePins();
        }

        private void exploreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(aeroListView1.SelectedItems[0].ImageKey);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(aeroListView1.SelectedItems[0].ImageKey);
        }

        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string FilePath = aeroListView1.SelectedItems[0].ImageKey;
            string DirPath = new FileInfo(FilePath).DirectoryName;

            Process.Start(DirPath);
        }

        public static void ShowOpenWithDialog(string path)
        {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + path;
            Process.Start("rundll32.exe", args);
        }

        private void openWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOpenWithDialog(aeroListView1.SelectedItems[0].ImageKey);
        }

        private void programSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pinIndex = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
            settings.Pins.RemoveAt(pinIndex);
            aeroListView1.Items.RemoveAt(pinIndex);
            SavePins();
        }

        private void toolStripSeparator7_Click(object sender, EventArgs e)
        {

        }
    }
}
