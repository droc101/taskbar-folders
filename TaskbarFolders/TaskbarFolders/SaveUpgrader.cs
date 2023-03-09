using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TaskbarFolders.Program;

namespace TaskbarFolders
{
    internal class SaveUpgrader
    {
        public struct SettingsV0
        {
            public string Name;
            public List<string> Pins;
            public string ImagePath;
            public Color color;
            public bool useColor;
        }

        public static int GetSaveVersion(string settings)
        {
            if (settings.StartsWith("["))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public GlobalSettings UpgradeV0toV1(List<SettingsV0> v0)
        {
            GlobalSettings currentSettings = new GlobalSettings();
            currentSettings.Folders = new List<Folder> { };
            currentSettings.DataVersion = 1;
            foreach (SettingsV0 old_folder in v0)
            {
                Folder folder = new Folder();
                folder.Pins = new List<Pin>();
                folder.Name = old_folder.Name;
                folder.ImagePath = old_folder.ImagePath;
                folder.color= old_folder.color;
                folder.useColor= old_folder.useColor;
                folder.Tags = new List<Tag>();

                foreach (string oldPin in old_folder.Pins)
                {
                    Pin pin = new Pin();
                    pin.Path= oldPin;
                    pin.Tags = new List<string>();
                    folder.Pins.Add(pin);
                }

                currentSettings.Folders.Add(folder);
            }
            return currentSettings;
        }

    }
}
