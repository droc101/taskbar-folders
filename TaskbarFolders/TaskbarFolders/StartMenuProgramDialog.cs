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
            try
            {
                foreach (KeyValuePair<string, string> program in GetPrograms(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu)))
                {
                    AddPinnedItem(program);
                }
            } catch (Exception ex)
            {

            }
            foreach (KeyValuePair<string, string> program in GetPrograms(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
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
                desc = new FileInfo(filePath).Name.Replace(".lnk", "");
            }
            return desc;
        }

        public void AddPinnedItem(KeyValuePair<string,string> dict)
        {
            string desc = GetItemDesc(dict.Key);
            ListViewItem lvi = new ListViewItem();
            lvi.Text = desc;
            Image bmp = IconUtils.GetLargestIcon(dict.Key);
            imageList1.Images.Add(dict.Key, bmp);
            lvi.ImageKey = dict.Key;
            lvi.ToolTipText = desc;
            aeroListView1.Items.Add(lvi);
        }

        public Dictionary<string, string> MergeDictionaries(Dictionary<string, string> dict1, Dictionary<string, string> dict2)
        {
            // Create a new dictionary to hold the merged key-value pairs
            Dictionary<string, string> mergedDict = new Dictionary<string, string>();

            // Add all key-value pairs from the first dictionary
            foreach (KeyValuePair<string, string> kvp in dict1)
            {
                mergedDict.Add(kvp.Key, kvp.Value);
            }

            // Add all key-value pairs from the second dictionary, overwriting any duplicates
            foreach (KeyValuePair<string, string> kvp in dict2)
            {
                mergedDict[kvp.Key] = kvp.Value;
            }

            return mergedDict;
        }


        Dictionary<string, string> GetPrograms(string path)
        {
            Dictionary<string, string> programs = new Dictionary<string, string>();
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
                                programs.Add(file, target);
                            }
                        }
                    }
                }
            }
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                programs = MergeDictionaries(programs, GetPrograms(folder));
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
