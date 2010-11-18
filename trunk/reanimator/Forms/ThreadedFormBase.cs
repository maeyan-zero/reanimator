using System;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public class ThreadedFormBase : Form
    {
        delegate DialogResult ShowDialogCallback(CommonDialog dialog);
        protected DialogResult ShowDialog(CommonDialog dialog)
        {
            if (InvokeRequired)
            {
                ShowDialogCallback d = ShowDialog;
                return (DialogResult)Invoke(d, new Object[] { dialog });
            }

            return dialog.ShowDialog(this);
        }

        // This function name and its conflic is intentional so as to ensure the correct function is used.
        // If you want to use the "original" one, call from base System namespace, etc (like below).
        // Feel free to add different paramater versions at your own discrestion (I was lazy and didn't feel like doing them all).
        delegate DialogResult MessageBoxCallback1(string text);
        protected DialogResult MessageBox(string text)
        {
            if (InvokeRequired)
            {
                MessageBoxCallback1 d = MessageBox;
                return (DialogResult)Invoke(d, new Object[] { text });
            }

            return System.Windows.Forms.MessageBox.Show(text);
        }

        delegate DialogResult MessageBoxCallback4(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        protected DialogResult MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (InvokeRequired)
            {
                MessageBoxCallback4 d = MessageBox;
                return (DialogResult)Invoke(d, new Object[] { text, caption, buttons, icon });
            }

            return System.Windows.Forms.MessageBox.Show(text, caption, buttons, icon);
        }

        delegate void DisposeCallback();
        new protected void Dispose()
        {
            if (InvokeRequired)
            {
                DisposeCallback d = Dispose;
                Invoke(d);
            }
            else
            {
                if (!IsDisposed)
                {
                    base.Dispose();
                }
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // ThreadedFormBase
            // 
            ClientSize = new System.Drawing.Size(284, 264);
            Name = "ThreadedFormBase";
            ShowIcon = false;
            ShowInTaskbar = false;
            ResumeLayout(false);
        }
    }
}