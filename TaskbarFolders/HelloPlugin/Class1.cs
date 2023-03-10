using System.Drawing;
using System.Windows.Forms;
using TaskbarFolders;

namespace HelloPlugin
{
    public class HelloPlugin : TaskbarFolders.Extension
    {
        // Handler to add items to the global context menu
        public override ToolStripItem[] MainMenuHandler()
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            mi.Text = "Main Menu Entry";
            mi.Click += Mi_Click;
            return new ToolStripItem[] { mi };
        }

        // Handler to add items to the file context menu
        public override ToolStripItem[] FileMenuHandler()
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            mi.Text = "File Menu Entry";
            mi.Click += Mi_Click;
            return new ToolStripItem[] { mi };
        }

        // Handler to add items to the folder context menu
        public override ToolStripItem[] FolderMenuHandler()
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            mi.Text = "Folder menu Entry";
            mi.Click += Mi_Click;
            return new ToolStripItem[] { mi };
        }

        // Handler to modify pinned items
        public override void ItemHandler(ListViewItem item)
        {
            item.Text = "[" + item.Text + "]";
        }

        // Handler for the item context menu
        public override ToolStripItem[] ItemMenuHandler()
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            mi.Text = "Item menu Entry";
            mi.Click += Mi_Click;
            return new ToolStripItem[] { mi };
        }

        // Handler for folder creation
        public override void OnFolderLoad(Form1 folder)
        {
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = "Example folder load modification";
            lbl.Dock = DockStyle.Bottom;
            folder.Controls.Add(lbl);
            folder.Controls.SetChildIndex(lbl, 0);
        }

        // Properties page handler
        public override TabPage PropertiesHandler()
        {
            TabPage tp = new TabPage();
            tp.Text = "Plugin Page";
            Label lbl = new Label();
            lbl.AutoSize = false;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = "Example plugins page";
            lbl.Dock= DockStyle.Fill;
            tp.Controls.Add(lbl);
            return tp;
        }

        // Menu Item Click. Not part of plugin API.
        private void Mi_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Hello from the example plugin!");
        }
    }
}
