using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms
{
    public partial class TextureSheetPreview : Form
    {
        AtlasImageLoader loader;
        int position;

        public TextureSheetPreview()
        {
            InitializeComponent();

            position = 0;
        }

        public void SetAtlasImageLoader(AtlasImageLoader loader)
        {
            this.loader = loader;

            foreach (string key in loader.GetImageNames())
            {
                PictureBox box = new PictureBox();
                box.SizeMode = PictureBoxSizeMode.Zoom;
                box.Image = loader.GetImage(key);
                box.Size = box.Image.Size;
                position += box.Width + 3;
                box.Location = new Point(3, position);
                box.BorderStyle = BorderStyle.FixedSingle;

                panelImages.Controls.Add(box);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                loader.SaveImagesToFolder(folderBrowserDialog1.SelectedPath);
            }
        }
    }
}
