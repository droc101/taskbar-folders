using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace TaskbarFolders
{
    public static class Program
    {

        public struct Settings
        {
            public string Name;
            public List<string> Pins;
            public string ImagePath;
            public Color color;
            public bool useColor;
        }

        public static List<Settings> currentSettings;

        public static string SrttingsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TaskbarFoldersConfig.json";

        public static void LoadSettings()
        {
            if (File.Exists(SrttingsPath))
            {
                currentSettings = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Settings>>(File.ReadAllText(SrttingsPath));
            } else
            {
                currentSettings = new List<Settings>();
                var folder = new Settings();
                folder.Pins = new List<string>();
                folder.Name = "Main Folder";
                folder.color = Color.FromArgb(0, 120, 212);
                folder.useColor = true;
                folder.ImagePath = "";
                currentSettings.Add(folder);
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
            foreach (Settings settings in currentSettings) { 
                new Form1(settings, i).Show();
                i++;
            }

            Application.Run();
        }
    }
}
