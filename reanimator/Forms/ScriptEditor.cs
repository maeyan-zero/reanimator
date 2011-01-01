using System;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class ScriptEditor : Form
    {
        public ScriptEditor(DataGridViewCell dataGridViewCell)
        {
            InitializeComponent();

            _save_button.Enabled = false;
            _testCompile_button.Enabled = false;
            _scriptEditor_richTextBox.Text = (String)dataGridViewCell.Value;
        }
    }
}
