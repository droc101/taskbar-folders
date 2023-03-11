using System;
using System.Reflection;
using System.Windows.Forms;

namespace TaskbarFolders
{
    public class Extension // Base class for Plugins & Extensions. !!! NOT YET FINISHED !!!
    {

        public struct ExtensionInfo
        {
            public string Name;
            public string Description;
            public string Developer;
            public Version Version;
        }

        public virtual ExtensionInfo GetInfo()
        {
            ExtensionInfo info = new ExtensionInfo();
            info.Name = "Unknown Extension";
            info.Description = "This extension has not provided information.";
            return info;
        }

        public virtual void OnFolderLoad(Form1 folder) // Called after a folder loads.
        {

        }

        public virtual void OnPluginStart()
        {

        }

        public virtual ToolStripItem[] MainMenuHandler() // Main context menu handler. Placed at the bottom.
        {
            return new ToolStripItem[] { };
        }

        public virtual ToolStripItem[] ItemMenuHandler() // Item context menu handler. Placed above the main menu. End with a seperator
        {
            return new ToolStripItem[] { };
        }

        public virtual ToolStripItem[] FileMenuHandler() // File context menu handler. Placed at the top.
        {
            return new ToolStripItem[] { };
        }

        public virtual ToolStripItem[] FolderMenuHandler() // Folder context menu handler. Placed at the top.
        {
            return new ToolStripItem[] { };
        }

        public virtual void ItemHandler(ListViewItem item) // Called before a pin gets added to the UI.
        {
            
        }

        public virtual TabPage PropertiesHandler() // Called when the properties page is opening.
        {
            return null;
        }

        public virtual TabPage SettingsHandler() // Called when the settings page is loading
        {
            return null;
        }

    }
}
