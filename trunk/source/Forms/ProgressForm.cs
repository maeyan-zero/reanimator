using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Reanimator.Forms
{
    public partial class ProgressForm : ThreadedFormBase
    {
        public ProgressForm()
        {
            InitializeComponent();
        }

        public delegate void ParameterizedProgressThread(ProgressForm progressBar, Object param);

        ParameterizedProgressThread threadFunc;
        Object threadParam;
        Form owner;

        public ProgressForm(ParameterizedProgressThread func, Object param)
        {
            threadFunc = func;
            threadParam = param; //new Object[] {param, this};

            InitializeComponent();

            this.Disposed += new EventHandler(ProgressForm_Disposed);
        }

        private void ProgressForm_Disposed(Object sender, EventArgs e)
        {
            if (this.owner != null)
            {
                this.owner.Hide();
                this.owner.Show();
            }
        }

        private void Progress_Shown(Object sender, EventArgs e)
        {
            if (threadFunc != null)
            {
                Form f = sender as Form;
                if (f != null)
                {
                    this.owner = f.Owner;
                }

                Thread t = new Thread(ProgressThread);
                t.Start(threadParam);
            }
        }

        private void ProgressThread(Object param)
        {
            threadFunc.Invoke(this, param);
            this.Dispose();
        }

        delegate void ConfigBarCallback(int minimum, int maximum, int step);
        public void ConfigBar(int minimum, int maximum, int step)
        {
            if (this.InvokeRequired)
            {
                ConfigBarCallback d = new ConfigBarCallback(ConfigBar);
                this.Invoke(d, new Object[] { minimum, maximum, step });
            }
            else
            {
                progressBar.Minimum = minimum;
                progressBar.Maximum = maximum;
                progressBar.Step = step;
            }
        }

        delegate void SetLoadingTextCallback(String loadingText);
        public void SetLoadingText(String loadingText)
        {
            if (this.InvokeRequired)
            {
                SetLoadingTextCallback d = new SetLoadingTextCallback(SetLoadingText);
                this.Invoke(d, new Object[] { loadingText });
            }
            else
            {
                loadingTextLabel.Text = loadingText;
            }
        }

        delegate void SetCurrentItemTextCallback(String currentItem);
        public void SetCurrentItemText(String currentItem)
        {
            if (this.InvokeRequired)
            {
                SetCurrentItemTextCallback d = new SetCurrentItemTextCallback(SetCurrentItemText);
                this.Invoke(d, new Object[] { currentItem });
            }
            else
            {
                currentItemLabel.Text = currentItem;
            }

        }

        public void StepProgress()
        {
            progressBar.Increment(progressBar.Step);
        }

        // Each time the text is modified (a new item is completed) let the progressbar progress and refresh the form
        private void currentItemLabel_TextChanged(object sender, EventArgs e)
        {
            progressBar.PerformStep();
            this.Refresh();
        }

        // Returns the currentItemLabel for setting its text
        public Label GetItemLabel()
        {
            return currentItemLabel;
        }


    }
}
