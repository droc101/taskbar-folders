using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TaskbarFolders
{
    public class FlatRenderer : ToolStripProfessionalRenderer
    {
        public FlatRenderer() : base(new FlatColors()) { }

        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {

            e.Graphics.DrawString("", SystemFonts.MenuFont, SystemBrushes.MenuText, e.ImageRectangle.Location);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.Graphics.DrawString("⯈", SystemFonts.MenuFont, SystemBrushes.MenuText, e.ArrowRectangle.Location);
        }

    }

    public class FlatColors : ProfessionalColorTable
    {

        public override Color ToolStripDropDownBackground => SystemColors.Menu;
        public override Color ImageMarginGradientBegin => SystemColors.Menu;
        public override Color ImageMarginGradientMiddle => SystemColors.Menu;

        public override Color MenuBorder => SystemColors.ButtonShadow;

        public override Color MenuItemSelected => SystemColors.ButtonShadow;
        public override Color MenuItemBorder => SystemColors.ButtonShadow;

        public override Color SeparatorDark => SystemColors.ButtonShadow;
        public override Color SeparatorLight => SystemColors.ButtonShadow;
    }
}
