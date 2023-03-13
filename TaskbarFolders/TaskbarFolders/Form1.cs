using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Net.NetworkInformation;
using static TaskbarFolders.Program;
using System.Linq;
using System.Text.RegularExpressions;
using LibMaterial.Framework;

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
            this.settings = settings;
            this.settingsIndex = settingsIndex;
            UpdateLooks();
            Size = new Size(350, 222);
            fileContextItems = TransferMenuItems(fileContextMenu.Items);
            folderContextItems = TransferMenuItems(folderContextMenu.Items);
            mainContextItems= TransferMenuItems(mainContextMenu.Items);
            itemContextItems = TransferMenuItems(itemContextMenu.Items);
            foreach (Extension ex in Program.extensions)
            {
                ex.OnFolderLoad(this);
            }
            //ListViewExtensions.SetTransparentBackground(aeroListView1);
            //BackColor = SystemColors.Window;
            //Round.MicaWindow(this);
            //var CMS = SummonContextMenu();
            //notifyIcon1.ContextMenuStrip = CMS;
        }

        void UpdateLooks()
        {
            cueTextBox1.Cue = "Search " + settings.Name + "...";
            

            aeroListView1.Sorting = settings.sortMode;
            aeroListView1.Groups.Clear();
            switch (settings.groupingMode)
            {
                case GroupingMode.NONE: break;
                case GroupingMode.ITEM_TYPE:
                    ListViewGroup ff_group = new ListViewGroup();
                    ff_group.Header = "Files & Folders";
                    ListViewGroup p_group = new ListViewGroup();
                    p_group.Header = "Programs";
                    aeroListView1.Groups.Add(ff_group);
                    aeroListView1.Groups.Add(p_group);
                    break;
                case GroupingMode.TAG:
                    foreach (Tag tag in settings.Tags)
                    {
                        ListViewGroup tag_group = new ListViewGroup();
                        tag_group.Header = tag.Name;
                        aeroListView1.Groups.Add(tag_group);
                    }
                    break;
                default: break; // Treat as "None"
            }

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

            aeroListView1.Items.Clear();
            foreach (Program.Pin pin in settings.Pins)
            {
                AddPinnedItem(pin);
            }

            
        }

        private void Tmi_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem tmi = (ToolStripMenuItem)sender;
            Tag t = (Tag)(tmi.Tag);
            int idx = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
            Pin pin = settings.Pins[idx];
            if (!tmi.Checked)
            {
                pin.Tags.Remove(t.Name);
            } else
            {
                pin.Tags.Add(t.Name);
            }
            UpdateLooks();
            SavePins();
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
                var fi = new FileInfo(pin.Path);
                desc = Path.GetFileNameWithoutExtension(pin.Path);
                if (currentSettings.ShowFileExtensions)
                {
                    desc += fi.Extension;
                }
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

            if (!File.Exists(pin.Path))
            {
                if (!Directory.Exists(pin.Path)) {
                    // File or Folder missing; don't pin it
                    return;
                }
            }

            string filePath = pin.Path;
            string desc = GetItemDesc(pin);
            ListViewItem lvi = new ListViewItem();
            lvi.Text = desc;
            Image bmp = IconUtils.GetLargestIcon(filePath);
            bmp = IconUtils.ScaleBitmap((Bitmap)bmp, imageList1.ImageSize.Width, imageList1.ImageSize.Height);
            imageList1.Images.Add(filePath, bmp);
            lvi.ImageKey = filePath;
            switch (settings.groupingMode)
            {
                case GroupingMode.NONE: break;
                case GroupingMode.ITEM_TYPE:
                    if (filePath.EndsWith(".exe"))
                    {
                        lvi.Group = aeroListView1.Groups[1];
                    }
                    else
                    {
                        lvi.Group = aeroListView1.Groups[0];
                    }
                    break;
                case GroupingMode.TAG:
                    foreach (ListViewGroup g in aeroListView1.Groups)
                    {
                        if (pin.Tags.Count > 0)
                        {
                            try
                            {
                                Tag t = findTag(pin.Tags.Last()).Value;
                                if (g.Header == t.Name)
                                {
                                    lvi.Group = g;
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                    }
                    break;
                default: break; // Treat as "None"
            }
            
            lvi.ToolTipText = desc;
            if (pin.Tags.Count > 0)
            {
                try
                {
                    Tag t = findTag(pin.Tags.Last()).Value;
                    lvi.ForeColor = t.FontColor;
                    lvi.ToolTipText += Environment.NewLine + String.Join(", ", pin.Tags);
                } catch (Exception ex)
                {

                }
                
            }
            foreach (Extension ex in Program.extensions)
            {
                ex.ItemHandler(lvi);
            }
            aeroListView1.Items.Add(lvi);
        }

        Tag? findTag(string name)
        {
            foreach (Tag tag in settings.Tags)
            {
                if (tag.Name == name) {
                    return tag;
                }
            }
            return null;
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

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Location = Point.Subtract((Point)Screen.PrimaryScreen.WorkingArea.Size, Size);
                Location = Point.Subtract(Location, new Size(10, 10));
                cueTextBox1.ResetText();
                aeroListView1.Focus();
                Show();
                Activate();
                BringToFront();
            } else if (e.Button == MouseButtons.Right)
            {
                Form form = new Form();
                form.Opacity = 0;
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(999999, 999999);
                form.ShowInTaskbar = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Show();
                form.Activate();
                var CMS = SummonContextMenu();
                CMS.Closed += delegate
                {
                    Location = new Point(0, 0);
                    form.Close();
                };
                CMS.Show(Cursor.Position);
                CMS.Focus();
            }
        }

        ContextMenuStrip SummonContextMenu()
        {
            ContextMenuStrip CMS = new ContextMenuStrip();
            
            CMS.Renderer = new FlatRenderer();

            Round.RoundWindow(CMS.Handle);
            Round.RoundWindow(tagsToolStripMenuItem.DropDown.Handle);

            if (aeroListView1.SelectedItems.Count != 0 && Visible)
            {
                if (Directory.Exists(aeroListView1.SelectedItems[0].ImageKey))
                {
                    TransferMenuItems(folderContextItems, CMS.Items);

                    foreach (Extension ex in Program.extensions)
                    {
                        var MenuItems = ex.FolderMenuHandler().ToList();
                        if (MenuItems.Count > 0)
                        {
                            AddMenuSeperator(CMS.Items);
                            TransferMenuItems(MenuItems, CMS.Items);
                        }
                        
                    }
                    AddMenuSeperator(CMS.Items);
                }
                else
                {
                    TransferMenuItems(fileContextItems, CMS.Items);

                    foreach (Extension ex in Program.extensions)
                    {
                        var MenuItems = ex.FileMenuHandler().ToList();
                        if (MenuItems.Count > 0)
                        {
                            AddMenuSeperator(CMS.Items);
                            TransferMenuItems(MenuItems, CMS.Items);
                        }

                    }
                    AddMenuSeperator(CMS.Items);
                }
                tagsToolStripMenuItem.DropDownItems.Clear();
                if (settings.Tags.Count == 0)
                {
                    tagsToolStripMenuItem.Text = "No Tags";
                    tagsToolStripMenuItem.Enabled = false;
                }
                else
                {
                    tagsToolStripMenuItem.Enabled = true;
                    tagsToolStripMenuItem.Text = "&Tags";
                    foreach (Tag tag in settings.Tags)
                    {
                        int idx = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
                        Pin pin = settings.Pins[idx];
                        ToolStripMenuItem tmi = new ToolStripMenuItem();
                        tmi.Text = tag.Name;
                        tmi.ForeColor = tag.FontColor;
                        tmi.Tag = tag;
                        tmi.Checked = pin.Tags.Contains(tag.Name);
                        tmi.CheckOnClick = true;
                        tmi.CheckedChanged += Tmi_CheckedChanged;
                        tagsToolStripMenuItem.DropDownItems.Add(tmi);
                    }
                }

                TransferMenuItems(itemContextItems, CMS.Items);
                foreach (Extension ex in Program.extensions)
                {
                    var MenuItems = ex.ItemMenuHandler().ToList();
                    if (MenuItems.Count > 0)
                    {
                        AddMenuSeperator(CMS.Items);
                        TransferMenuItems(MenuItems, CMS.Items);
                    }

                }
                AddMenuSeperator(CMS.Items);
            }
            foreach (Extension ex in Program.extensions)
            {
                var MenuItems = ex.MainMenuHandler().ToList();
                if (MenuItems.Count > 0)
                {
                    
                    TransferMenuItems(MenuItems, CMS.Items);
                    AddMenuSeperator(CMS.Items);
                }

            }
            TransferMenuItems(mainContextItems, CMS.Items);
            CMS.AutoClose = true;
            return CMS;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }   

        private void cueTextBox1_TextChanged(object sender, EventArgs e)
        {
            aeroListView1.Items.Clear();
            if (cueTextBox1.Text.StartsWith("*"))
            {
                // Tag Search
                foreach (Program.Pin pin in settings.Pins)
                {
                    foreach (string tag in pin.Tags)
                    {
                        if (tag.ToLower().Contains(cueTextBox1.Text.TrimStart('*').ToLower()))
                        {
                            AddPinnedItem(pin);
                            break;
                        }
                    }
                }
            } else
            {
                // Name search
                foreach (Program.Pin pin in settings.Pins)
                {
                    if (GetItemDesc(pin).ToLower().Contains(cueTextBox1.Text.ToLower()) || String.IsNullOrEmpty(cueTextBox1.Text))
                    {
                        AddPinnedItem(pin);
                    }
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

        void AddMenuSeperator(ToolStripItemCollection to)
        {
            ToolStripSeparator sep = new ToolStripSeparator();
            to.Add(sep);
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
            Program.Folder newf = CreateDefaultFolder();
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
            if (taskDialog2.ShowDialog().ButtonType == Ookii.Dialogs.WinForms.ButtonType.Ok)
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
            
        }

        private void exitProgramToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (taskDialog1.ShowDialog().ButtonType == Ookii.Dialogs.WinForms.ButtonType.Ok)
            {
                SavePins();
                notifyIcon1.Visible = false;
                Process.GetCurrentProcess().Kill();
            }
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
            SettingsForm sf = new SettingsForm();
            if (sf.ShowDialog() == DialogResult.OK)
            {
                UpdateLooks();
            }
        }

        private void removeItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pinIndex = FindPinIndex(aeroListView1.SelectedItems[0].ImageKey);
            settings.Pins.RemoveAt(pinIndex);
            aeroListView1.Items.RemoveAt(pinIndex);
            UpdateLooks();
            SavePins();
        }

        private void addProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartMenuProgramDialog pd = new StartMenuProgramDialog();
            if (pd.ShowDialog() == DialogResult.OK)
            {
                if (IsPinned(pd.path))
                {
                    MessageBox.Show("This item is already pinned.", "Can't pin", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                AddPinnedItem(pd.path);
                settings.Pins.Add(CreateDefaultPin(pd.path));
                SavePins();
            }
        }

        private void aeroListView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SummonContextMenu().Show(Cursor.Position);
            }
        }
    }
}
