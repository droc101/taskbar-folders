using System.Windows.Forms;

namespace HelloPlugin
{
    public class HelloPlugin : TaskbarFolders.Extension
    {
        public override ToolStripItem[] MainMenuHandler()
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            mi.Text = "Hello, Plugin World!";
            mi.Click += Mi_Click;
            return new ToolStripItem[] { mi };
        }

        private void Mi_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Hello from the Main Menu!");
        }
    }
}
