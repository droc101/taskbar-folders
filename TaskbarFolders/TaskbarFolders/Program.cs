using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Linq;

namespace TaskbarFolders
{
    public static class Program
    {

        public static List<Extension> extensions = new List<Extension>();

        public struct Folder
        {
            public string Name;
            public List<Pin> Pins;
            public List<Tag> Tags;
            public string ImagePath;
            public Color color;
            public bool useColor;
            public GroupingMode groupingMode;
            public SortOrder sortMode;
        }

        public enum GroupingMode
        {
            NONE,
            ITEM_TYPE,
            TAG
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
        public static string PluginsPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\TaskbarFolderPlugins\";
        static string PluginWarningText = "WARNING: Plugins are written in C# and can potentially be malicious. Please install plugins with care.";

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
                var folder = CreateDefaultFolder();
                folder.Name = "Main Folder";
                currentSettings.Folders.Add(folder);
                SaveSettings();
                Welcome welcome = new Welcome();
                welcome.ShowDialog();
            }
            
        }

        public static Folder CreateDefaultFolder()
        {
            var folder = new Folder();
            folder.Pins = new List<Pin>();
            folder.Name = "New Folder";
            folder.color = Color.FromArgb(0, 120, 212);
            folder.useColor = true;
            folder.ImagePath = "";
            return folder;
        }

        static IEnumerable<Extension> GetAllExtensions()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(Extension)))
                .Select(type => Activator.CreateInstance(type) as Extension);
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
            if (!Directory.Exists(PluginsPath))
            {
                Directory.CreateDirectory(PluginsPath);
                File.WriteAllText(PluginsPath + "README.txt", PluginWarningText);
            }
            foreach (string Plugin in Directory.EnumerateFiles(PluginsPath))
            {
                if (Plugin.EndsWith(".dll"))
                {
                    Assembly.LoadFile(Plugin);
                }
                
            }
            GetAllExtensions().ToList().ForEach(x =>
            {
                extensions.Add(x);
            });
            LoadSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            foreach (Extension extension in extensions)
            {
                extension.OnPluginStart();
            }
            int i = 0;
            foreach (Folder settings in currentSettings.Folders) { 
                new Form1(settings, i).Show();
                i++;
            }

            Application.Run();
        }
    }
}
