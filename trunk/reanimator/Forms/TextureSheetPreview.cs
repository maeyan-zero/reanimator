using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Forms.HeroEditorFunctions;
using System.IO;

namespace Reanimator.Forms
{
    public partial class TextureSheetPreview : Form
    {
        AtlasImageLoader loader;
        Bitmap bmp;
        string name;

        public TextureSheetPreview()
        {
            InitializeComponent();
        }

        public void SetAtlasImageLoader(AtlasImageLoader loader)
        {
            this.loader = loader;

            comboBox1.DataSource = loader.GetImageNames();
        }

        public void SetBitmap(Bitmap bmp, string name)
        {
            this.bmp = bmp;
            this.name = name;
            comboBox1.Enabled = false;

            pictureBox1.Image = bmp;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (loader != null)
                {
                    loader.SaveImagesToFolder(folderBrowserDialog1.SelectedPath);
                }
                else
                {
                    string path = Path.Combine(folderBrowserDialog1.SelectedPath, name + ".bmp");
                    bmp.Save(path);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bmp = loader.GetImage((string)comboBox1.SelectedItem);

            pictureBox1.Image = bmp;
        }
    }
}
