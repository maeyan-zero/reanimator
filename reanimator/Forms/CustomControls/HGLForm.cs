using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms.CustomControls
{
    public partial class HGLForm : Form
    {
        FileManager fileManager;
        

        public HGLForm(FileManager fileManager)
        {
            InitializeComponent();

            this.fileManager = fileManager;
        }
    }
}
