using System;
using System.Windows.Forms;
using System.Threading;

namespace Reanimator.Forms
{
    public partial class ProgressForm : ThreadedFormBase
    {
        public ProgressForm()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public delegate void ParameterizedProgressThread(ProgressForm progressBar, Object param);

        readonly ParameterizedProgressThread _threadFunc;
        readonly Object _threadParam;
        Form _owner;

        public ProgressForm(ParameterizedProgressThread func, Object param) : this()
        {
            _threadFunc = func;
            _threadParam = param;

            this.Disposed += ProgressForm_Disposed;
        }

        private void ProgressForm_Disposed(Object sender, EventArgs e)
        {
            if (this._owner == null) return;

            this._owner.Hide();
            this._owner.Show();
        }

        private void Progress_Shown(Object sender, EventArgs e)
        {
            if (_threadFunc == null) return;

            Form f = sender as Form;
            if (f != null)
            {
                this._owner = f.Owner;
            }

            Thread t = new Thread(ProgressThread);
            t.Start(_threadParam);
        }

        private void ProgressThread(Object param)
        {
            _threadFunc.Invoke(this, param);
            this.Dispose();
        }

        delegate void ConfigBarCallback(int minimum, int maximum, int step);
        public void ConfigBar(int minimum, int maximum, int step)
        {
            if (this.InvokeRequired)
            {
                ConfigBarCallback d = ConfigBar;
                this.Invoke(d, new Object[] { minimum, maximum, step });
            }
            else
            {
                // It'll throw an exception if you try to change outside of its range a second time without resetting the value
                progressBar.Value = progressBar.Minimum;
                progressBar.Minimum = minimum;
                progressBar.Maximum = maximum;
                progressBar.Step = step;
                progressBar.Value = minimum;
            }
        }

        delegate void SetLoadingTextCallback(String loadingText);
        public void SetLoadingText(String loadingText)
        {
            if (this.InvokeRequired)
            {
                SetLoadingTextCallback d = SetLoadingText;
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
                SetCurrentItemTextCallback d = SetCurrentItemText;
                this.Invoke(d, new Object[] { currentItem });
            }
            else
            {
                currentItemLabel.Text = currentItem;
            }
        }

        delegate void StepProgressCallback();
        public void StepProgress()
        {
            if (this.InvokeRequired)
            {
                StepProgressCallback d = StepProgress;
                this.Invoke(d);
            }
            else
            {
                if (progressBar.Style != ProgressBarStyle.Marquee)
                {
                    progressBar.Increment(progressBar.Step);
                }
            }
        }

        delegate void SetStyleCallback(ProgressBarStyle progressBarStyle);
        public void SetStyle(ProgressBarStyle progressBarStyle)
        {
            if (this.InvokeRequired)
            {
                SetStyleCallback d = SetStyle;
                this.Invoke(d, new Object[] { progressBarStyle });
            }
            else
            {
                progressBar.Style = progressBarStyle;
            }
        }

        // Each time the text is modified (a new item is completed) let the progressbar progress and refresh the form
        private void currentItemLabel_TextChanged(object sender, EventArgs e)
        {
            this.StepProgress();
            this.Refresh();
        }
    }
}
