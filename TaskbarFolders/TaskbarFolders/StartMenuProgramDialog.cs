using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Shell32;
using static TaskbarFolders.Program;
using System.IO;
using System.Diagnostics;

namespace TaskbarFolders
{
    public partial class StartMenuProgramDialog : Form
    {
        public string path = null;

        public StartMenuProgramDialog()
        {
            InitializeComponent();
            foreach (string program in GetPrograms(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
            {
                AddPinnedItem(program);
            }
            foreach (string program in GetPrograms(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)))
            {
                AddPinnedItem(program);
            }
        }

        string GetItemDesc(string filePath)
        {
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
            lvi.ToolTipText = desc;
            aeroListView1.Items.Add(lvi);
        }

        List<string> GetPrograms(string path)
        {
            List<string> programs = new List<string>();
            foreach (string file in Directory.EnumerateFiles(path))
            {
                if (file.EndsWith(".lnk"))
                {
                    string target = GetShortcutTargetFile(file);
                    if (target != null)
                    {
                        if (target.EndsWith(".exe"))
                        {
                            if (File.Exists(target))
                            {
                                programs.Add(target);
                            }
                        }
                    }
                }
            }
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                programs.AddRange(GetPrograms(folder));
            }
            return programs;
        }

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Shell32.Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (aeroListView1.SelectedItems.Count > 0)
            {
                DialogResult= DialogResult.OK;
            } else
            {
                MessageBox.Show("You must select an item.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void aeroListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (aeroListView1.SelectedItems.Count > 0)
            {
                path = aeroListView1.SelectedItems[0].ImageKey;
            }
        }
    }
}
