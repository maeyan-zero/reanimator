using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class CharacterSkillsControl : UserControl
    {
        public CharacterSkillsControl()
        {
            InitializeComponent();
        }

        public void SetBackground(int characterClassID)
        {
            //characterClassID = 3;
            Bitmap skillPanelImage = (Bitmap)Bitmap.FromFile(Directory.GetCurrentDirectory() + @"\images\" + characterClassID + ".bmp");

            SetBackground(skillPanelImage);
        }

        public void SetBackground(Bitmap skillBackground)
        {
            //this.BackgroundImage = global::Reanimator.Properties.Resources.skillPanelBg;
            skillBackground.MakeTransparent(Color.White);

            Bitmap bg = (Bitmap)this.BackgroundImage;
            Graphics g = Graphics.FromImage(bg);

            g.DrawImage(skillBackground, new Point());

            g.Dispose();

            this.BackgroundImage = bg;
            //this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
