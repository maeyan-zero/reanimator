using System;
using System.Drawing;
using System.Windows.Forms;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class ScriptEditor : Form
    {
        private readonly FileManager _fileManager;
        private readonly DataGridViewCell _dataGridViewCell;
        private bool _textChanged;

        public ScriptEditor(FileManager fileManager, DataGridViewCell dataGridViewCell, String rowName, String colName)
        {
            InitializeComponent();

            _fileManager = fileManager;
            _dataGridViewCell = dataGridViewCell;
            Text = String.Format("Script Editor: Row({0}) '{1}', Col({2}) '{3}'", dataGridViewCell.RowIndex, rowName, dataGridViewCell.ColumnIndex, colName);

            FontFamily fontFamily = new FontFamily("Courier New");
            Font font = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Point);
            _scriptEditor_RichTextBox.Font = font;
            _scriptEditor_RichTextBox.Text = (String)dataGridViewCell.Value;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void _TestCompile_Button_Click(object sender, EventArgs e)
        {
            ExcelScript script = new ExcelScript(_fileManager);
            try
            {
                script.Compile(_scriptEditor_RichTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to compile script!\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void _Save_Button_Click(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;

            if (_textChanged)
            {
                dr = MessageBox.Show("Changes to the script have been made, attempt to compile the script and save befor exiting?", "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            }
            if (dr == DialogResult.Cancel) return;
            if (dr == DialogResult.No)
            {
                Close();
                return;
            }

            ExcelScript script = new ExcelScript(_fileManager);
            try
            {
                script.Compile(_scriptEditor_RichTextBox.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to compile script!\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                dr = MessageBox.Show("Do you wish to still quit?", "Quit without saving?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;
                if (dr == DialogResult.Yes)
                {
                    Close();
                    return;
                }
            }

            _dataGridViewCell.Value = _scriptEditor_RichTextBox.Text;
        }

        private void _ScriptEditor_RichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_textChanged) return;

            Text += "*";
            _textChanged = true;
        }
    }
}
