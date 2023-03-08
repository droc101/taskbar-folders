using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskbarFolders
{
    public partial class EditFolderDialog : Form
    {

        public Program.Settings settings;

        public EditFolderDialog(Program.Settings settings)
        {
            InitializeComponent();
            this.settings= settings;
            cueTextBox1.Text = settings.Name;
            cueTextBox2.Text = settings.ImagePath;
            radioButton1.Checked = !settings.useColor;
            radioButton2.Checked = settings.useColor;
            panel1.BackColor = settings.color;
            UpdateLooks();
        }

        private void EditFolderDialog_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(cueTextBox2.Text) && radioButton1.Checked)
            {
                MessageBox.Show("Invalid Folder Image Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            settings.Name= cueTextBox1.Text;
            settings.ImagePath = cueTextBox2.Text;
            settings.color = panel1.BackColor;
            settings.useColor = radioButton2.Checked;
            DialogResult= DialogResult.OK;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        void UpdateLooks()
        {
            if (settings.useColor)
            {
                Bitmap tint = IconUtils.ApplyTint(Properties.Resources.generics_folder_png, settings.color, 0.5f);
                Icon = Icon.FromHandle(tint.GetHicon());
                pictureBox1.Image = tint;
            }
            else
            {
                Icon = Properties.Resources.generics_folder;
                pictureBox1.Image = Properties.Resources.generics_folder_png;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel1.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
                UpdateLooks();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cueTextBox2.Text = openFileDialog1.FileName;
            }
        }
    }
}
