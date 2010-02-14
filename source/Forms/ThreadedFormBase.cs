using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public class ThreadedFormBase : Form
    {
        delegate DialogResult ShowDialogCallback(CommonDialog dialog);
        protected DialogResult ShowDialog(CommonDialog dialog)
        {
            if (this.InvokeRequired)
            {
                ShowDialogCallback d = new ShowDialogCallback(ShowDialog);
                return (DialogResult)this.Invoke(d, new Object[] { dialog });
            }
            else
            {
                return dialog.ShowDialog(this);
            }
        }

        // This function name and its conflic is intentional so as to ensure the correct function is used.
        // If you want to use the "original" one, call from base System namespace, etc (like below).
        // Feel free to add different paramater versions at your own discrestion (I was lazy and didn't feel like doing more than this one).
        delegate DialogResult MessageBoxCallback(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        protected DialogResult MessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (this.InvokeRequired)
            {
                MessageBoxCallback d = new MessageBoxCallback(MessageBox);
                return (DialogResult)this.Invoke(d, new Object[] { text, caption, buttons, icon });
            }
            else
            {
                return System.Windows.Forms.MessageBox.Show(text, caption, buttons, icon);
            }
        }

        delegate void DisposeCallback();
        new protected void Dispose()
        {
            if (this.InvokeRequired)
            {
                DisposeCallback d = new DisposeCallback(Dispose);
                this.Invoke(d);
            }
            else
            {
                base.Dispose();
            }
        }
    }
}