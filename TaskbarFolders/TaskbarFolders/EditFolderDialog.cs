using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.ListViewItem;
using static TaskbarFolders.Program;

namespace TaskbarFolders
{
    public partial class EditFolderDialog : Form
    {

        public Program.Folder settings;

        public EditFolderDialog(Program.Folder settings)
        {
            InitializeComponent();
            this.settings= settings;
            cueTextBox1.Text = settings.Name;
            cueTextBox2.Text = settings.ImagePath;
            radioButton1.Checked = !settings.useColor;
            radioButton2.Checked = settings.useColor;
            panel1.BackColor = settings.color;
            UpdateTagLV();
            UpdateLooks();
            foreach (Extension ex in Program.extensions)
            {
                TabPage exp = ex.PropertiesHandler();
                if (exp != null)
                {
                    tabControl1.TabPages.Add(exp);
                }
            }
        }

        void UpdateTagLV()
        {
            aeroListView1.Items.Clear();
            foreach (Tag tag in settings.Tags)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = tag.Name;
                ListViewSubItem visi = new ListViewSubItem();
                visi.Text = HexConverter(tag.FontColor);
                lvi.ForeColor = tag.FontColor;
                lvi.SubItems.Add(visi);
                aeroListView1.Items.Add(lvi);
            }
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
            settings.Tags.Clear();
            foreach (ListViewItem lvi in aeroListView1.Items)
            {
                Tag tag = new Tag();
                tag.Name = lvi.Text;
                tag.FontColor = lvi.ForeColor;
                settings.Tags.Add(tag);
            }
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

        private static String HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Tag tag = new Tag();
            tag.Name = "New Tag";
            tag.FontColor = SystemColors.WindowText;
            TagEditDialog ted = new TagEditDialog(tag);
            if (ted.ShowDialog() == DialogResult.OK)
            {
                if (doesTagExist(ted.tag.Name))
                {
                    MessageBox.Show("That tag already exists", "Can't add tag", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                }
                settings.Tags.Add(ted.tag);
                UpdateTagLV();
            }
        }

        bool doesTagExist(string tag)
        {
            foreach (ListViewItem lvi in aeroListView1.Items)
            {
                if (lvi.Text == tag) return true;
            }
            return false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (aeroListView1.SelectedItems.Count > 0)
            {
                aeroListView1.Items.RemoveAt(aeroListView1.SelectedIndices[0]);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (aeroListView1.SelectedItems.Count > 0)
            {
                int index = aeroListView1.SelectedIndices[0];
                Tag tag = new Tag();
                tag.Name = aeroListView1.Items[index].Text;
                tag.FontColor = aeroListView1.Items[index].ForeColor;
                TagEditDialog ted = new TagEditDialog(tag);
                if (ted.ShowDialog() == DialogResult.OK)
                {
                    if (doesTagExist(ted.tag.Name) && ted.tag.Name != tag.Name)
                    {
                        MessageBox.Show("That tag already exists", "Can't add tag", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
                    }
                    aeroListView1.Items[index].Text = ted.tag.Name;
                    aeroListView1.Items[index].ForeColor = ted.tag.FontColor;
                }
            }
            
        }
    }
}
