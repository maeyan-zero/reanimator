using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Forms.HeroEditorFunctions;

namespace Reanimator.Forms.CustomControls
{
    public partial class HGLForm : Form
    {
        FileManager fileManager;
        HGLWindowBackground background;

        public HGLForm(FileManager fileManager)
        {
            InitializeComponent();

            this.fileManager = fileManager;

            background = new HGLWindowBackground();

            comboBoxWindowStyle.DataSource = Enum.GetNames(typeof(HGLWindowStyle));
            comboBoxWindowStyle.SelectedIndex = 0;
        }

        private void HGLForm_Resize(object sender, EventArgs e)
        {
            if (this.BackgroundImage != null)
            {
                this.BackgroundImage.Dispose();
            }

            this.BackgroundImage = background.GenerateBackground(this.ClientSize);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            HGLWindowStyle style = (HGLWindowStyle)Enum.Parse(typeof(HGLWindowStyle), comboBoxWindowStyle.SelectedValue.ToString());
            background.SetBitmaps(fileManager, style);

            HGLForm_Resize(null, null);
        }
    }
}
