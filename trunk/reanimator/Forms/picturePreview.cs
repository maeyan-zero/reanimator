using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class PicturePreview : Form
    {
        public PicturePreview()
        {
            InitializeComponent();
            
        }

        //gets the image from FileExplorer
        public void setImage(Bitmap image)
        {
            pictureBox1.Image = image;
        }
    }
}
