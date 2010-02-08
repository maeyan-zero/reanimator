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