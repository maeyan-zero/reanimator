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
            skillPanelImage.MakeTransparent(Color.White);
            this.BackgroundImage = skillPanelImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        public void SetBackground(Bitmap skillBackground)
        {
            Bitmap skillPanelImage = skillBackground;
            skillPanelImage.MakeTransparent(Color.White);
            this.BackgroundImage = skillPanelImage;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
