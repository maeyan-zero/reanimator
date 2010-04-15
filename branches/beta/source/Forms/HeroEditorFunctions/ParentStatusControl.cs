using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Properties;

namespace Reanimator.Forms.HeroEditorFunctions
{
    public partial class ParentStatusControl : UserControl
    {
        Size _maximizedSize;
        Size _minimizedSize;
        bool _isMaximized;

        public Size MinimizedSize
        {
            get { return _minimizedSize; }
        }

        public Size MaximizedSize
        {
            get { return _maximizedSize; }
            set
            {
                _maximizedSize = value;
            }
        }

        public ParentStatusControl()
            : base()
        {
            InitializeComponent();

            _minimizedSize = new Size(256, 27);

            _isMaximized = true;
        }

        private void b_minimizeMaximize_Click(object sender, EventArgs e)
        {
            if (_isMaximized)
            {
                b_minimizeMaximize.BackgroundImage = Resources.panelButton_minimize_small;
                this.Size = _minimizedSize;

                foreach (Control control in this.Controls)
                {
                    string tag = (string)control.Tag;
                    if (tag != "header")
                    {
                        control.Visible = false;
                    }
                }
            }
            else
            {
                b_minimizeMaximize.BackgroundImage = Resources.panelButton_maximize_small;
                this.Size = _maximizedSize;

                foreach (Control control in this.Controls)
                {
                    control.Visible = true;
                }
            }

            _isMaximized = !_isMaximized;
        }

        public void SetLabelText(string text)
        {
            l_controlMenuName.Text = text;
        }

        private void ParentStatusControl_Click(object sender, EventArgs e)
        {
            this.Parent.Select();
        }
    }
}
