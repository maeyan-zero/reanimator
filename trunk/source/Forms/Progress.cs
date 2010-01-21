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
    public partial class Progress : Form
    {
        public Progress()
        {
            InitializeComponent();
        }

        public void ConfigBar(int minimum, int maximum, int step)
        {
            progressBar1.Minimum = minimum;
            progressBar1.Maximum = maximum;
            progressBar1.Step = step;
        }

        public void SetLoadingText(string szText)
        {
            loadingTextLabel.Text = szText;
        }

        public void SetCurrentItemText(string szText)
        {
            currentItemLabel.Text = szText;
        }

        public void StepProgress()
        {
            progressBar1.Increment(progressBar1.Step);
        }
    }
}
