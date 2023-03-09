using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace TaskbarFolders
{
    public static class Program
    {

        public struct Folder
        {
            public string Name;
            public List<Pin> Pins;
            public List<Tag> Tags;
            public string ImagePath;
            public Color color;
            public bool useColor;
        }

        public struct GlobalSettings
        {
            public List<Folder> Folders;
            public int DataVersion;
        }

        public struct Pin
        {
            public string Path;
            public List<string> Tags;
        }

        public struct Tag
        {
            public string Name;
            public Color FontColor;
        }

        public static GlobalSettings currentSettings;

        public static string SrttingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TaskbarFoldersConfig.json";

        public static void LoadSettings()
        {
            if (File.Exists(SrttingsPath))
            {
                int SettingsVersion = SaveUpgrader.GetSaveVersion(File.ReadAllText(SrttingsPath));
                if (SettingsVersion == 0)
                {
                    SaveUpgrader su = new SaveUpgrader();
                    currentSettings = su.UpgradeV0toV1(Newtonsoft.Json.JsonConvert.DeserializeObject<List<SaveUpgrader.SettingsV0>>(File.ReadAllText(SrttingsPath)));
                    SaveSettings();
                } else
                {
                    currentSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<GlobalSettings>(File.ReadAllText(SrttingsPath));
                }
            } else
            {
                currentSettings = new GlobalSettings();
                currentSettings.Folders = new List<Folder> {};
                currentSettings.DataVersion = 1;
                var folder = new Folder();
                folder.Pins = new List<Pin>();
                folder.Name = "Main Folder";
                folder.color = Color.FromArgb(0, 120, 212);
                folder.useColor = true;
                folder.ImagePath = "";
                currentSettings.Folders.Add(folder);
                SaveSettings();
            }
            
        }

        public static void SaveSettings()
        {
            string ss = Newtonsoft.Json.JsonConvert.SerializeObject(currentSettings);
            File.WriteAllText(SrttingsPath, ss);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoadSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            int i = 0;
            foreach (Folder settings in currentSettings.Folders) { 
                new Form1(settings, i).Show();
                i++;
            }

            Application.Run();
        }
    }
}
