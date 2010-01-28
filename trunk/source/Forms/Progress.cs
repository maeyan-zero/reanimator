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

        // Each time the text is modified (a new item is completed) let the progressbar progress and refresh the form
        private void currentItemLabel_TextChanged(object sender, EventArgs e)
        {
          progressBar1.PerformStep();
          this.Refresh();
        }

        // Returns the currentItemLabel for setting its text
        public Label GetItemLabel()
        {
          return currentItemLabel;
        }
    }
}
