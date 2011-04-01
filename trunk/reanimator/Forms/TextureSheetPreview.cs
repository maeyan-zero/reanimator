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

        public TextureSheetPreview()
        {
            InitializeComponent();
        }

        public void SetAtlasImageLoader(AtlasImageLoader loader)
        {
            this.loader = loader;

            comboBox1.DataSource = loader.GetImageNames();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                loader.SaveImagesToFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bmp = loader.GetImage((string)comboBox1.SelectedItem);

            pictureBox1.Image = bmp;
        }
    }
}
