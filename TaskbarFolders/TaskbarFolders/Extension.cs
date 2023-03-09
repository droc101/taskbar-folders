using System.Windows.Forms;

namespace TaskbarFolders
{
    public class Extension // Base class for Plugins & Extensions. !!! NOT YET FINISHED !!!
    {

        public virtual void OnFolderLoad(Form1 folder) // Called after a folder loads.
        {

        }

        public virtual ToolStripItem[] MainMenuHandler() // Main context menu handler. Placed at the bottom.
        {
            return null;
        }

        public virtual ToolStripItem[] ItemMenuHandler() // Item context menu handler. Placed above the main menu. End with a seperator
        {
            return null;
        }

        public virtual ToolStripItem[] FileMenuHandler() // File context menu handler. Placed at the top.
        {
            return null;
        }

        public virtual ToolStripItem[] FolderMenuHandler() // Folder context menu handler. Placed at the top.
        {
            return null;
        }

        public virtual void ItemHandler(ListViewItem item) // Called before a pin gets added to the UI.
        {
            
        }

        public virtual TabPage PropertiesHandler() // Called when the properties page is opening.
        {
            return null;
        }

    }
}
