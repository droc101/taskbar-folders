using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TaskbarFolders.Program;

namespace TaskbarFolders
{
    public partial class TagEditDialog : Form
    {
        public Tag tag;

        public TagEditDialog(Tag tag)
        {
            InitializeComponent();
            this.tag = tag;
            cueTextBox1.Text = tag.Name;
            panel1.BackColor = tag.FontColor;
        }

        private void TagEditDialog_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = panel1.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tag.Name = cueTextBox1.Text;
            tag.FontColor = panel1.BackColor;
            DialogResult = DialogResult.OK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
