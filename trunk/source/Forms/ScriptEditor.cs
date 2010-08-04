using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms
{
    public partial class ScriptEditor : Form
    {
        UTF8Encoding _encoding; //used for string to byte[]
        TableDataSet _tableDataSet;
        List<string> _availableFiles;
        int _currentTable = -1;

        public string ScriptDir { get { return Config.HglDir + "\\Reanimator\\Scripts"; } }
        public string Filter { get { return "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*"; } }

        public ScriptEditor(TableDataSet tableDataSet)
        {
            _encoding = new System.Text.UTF8Encoding();
            _availableFiles = new List<string>(Directory.GetFiles(ScriptDir, "*.xml"));
            _tableDataSet = tableDataSet;

            InitializeComponent();
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Config.HglDir) && !Directory.Exists(ScriptDir))
            {
                Directory.CreateDirectory(ScriptDir);
            }
            else
            {
                string[] filenames = FileTools.FileNameFromPath(_availableFiles.ToArray());
                availableCombo.Items.AddRange(filenames);
            }
        }

        private void availableCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            savePrompt();
            if (_currentTable != availableCombo.SelectedIndex)
            {
                int i = availableCombo.SelectedIndex;
                using (FileStream fileStream = new FileStream(@_availableFiles[i], FileMode.Open))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    textBox.Text = _encoding.GetString(buffer);
                    _currentTable = i;
                }
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            byte[] buffer = _encoding.GetBytes(textBox.Text);

            if (_currentTable == -1)
            {
                SaveFileDialog saveFileDialogue = new SaveFileDialog();
                saveFileDialogue.InitialDirectory = ScriptDir;
                saveFileDialogue.Filter = Filter;
                saveFileDialogue.ShowDialog();

                //write new file stream if user has choosen a filename
                if (saveFileDialogue.FileName != "")
                {
                    using (FileStream fileStream = new FileStream(@saveFileDialogue.FileName, FileMode.Create))
                    {
                        fileStream.Write(buffer, 0, buffer.Length);
                    }

                    textBox.Modified = false;
                    string filename = FileTools.FileNameFromPath(saveFileDialogue.FileName);
                    availableCombo.Items.Add(filename);
                    _availableFiles.Add(saveFileDialogue.FileName);
                    _currentTable = availableCombo.Items.Count - 1;
                    availableCombo.SelectedIndex = _currentTable;
                }
            }
            else
            {
                using (FileStream fileStream = new FileStream(@_availableFiles[_currentTable], FileMode.Create))
                {
                    fileStream.Write(buffer, 0, buffer.Length);
                    textBox.Modified = false;
                }
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            _currentTable = -1;
            textBox.Text = "";
            availableCombo.SelectedIndex = -1;
        }

        private bool savePrompt()
        {
            if (textBox.Modified)
            {
                if (_currentTable == -1)
                {
                    DialogResult result = MessageBox.Show("Do you want to save this script? Note: a script must be saved before it can be applied.",
                        "New Script.", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        SaveFileDialog saveFileDialogue = new SaveFileDialog();
                        saveFileDialogue.InitialDirectory = ScriptDir;
                        saveFileDialogue.Filter = Filter;
                        saveFileDialogue.ShowDialog();

                        if (saveFileDialogue.FileName != "")
                        {
                            using (FileStream stream = new FileStream(@saveFileDialogue.FileName, FileMode.Create))
                            {
                                byte[] buffer = _encoding.GetBytes(textBox.Text);

                                stream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    DialogResult result = MessageBox.Show("Do you want to save the changes?",
                        "Changes have been made.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        byte[] buffer = _encoding.GetBytes(textBox.Text);

                        using (FileStream fileStream = new FileStream(@_availableFiles[_currentTable], FileMode.Create))
                        {
                            fileStream.Write(buffer, 0, buffer.Length);
                        }
                        return true;
                    }
                }
                return false; // its modified but not saved
            }
            else
            {
                return true; // wasn't modified, okay
            }
        }

        private void apply(int i)
        {
            Modification modification = new Modification(_tableDataSet);
            string path = _availableFiles[i];
            ProgressForm progress = new ProgressForm(modification.Open, path);
            progress.ShowDialog();
            progress = new ProgressForm(modification.Apply, modification);
            progress.ShowDialog();
        }

        private void applyCurrentButton_Click(object sender, EventArgs e)
        {
            bool okay = savePrompt();
            if (okay) apply(availableCombo.SelectedIndex);
        }

        private void applyCheckedButton_Click(object sender, EventArgs e)
        {
            foreach (int i in availableCombo.CheckedIndices)
            {
                bool okay = savePrompt();
                if (okay) apply(i);
            }
        }

        private void ScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            savePrompt();
        }
    }
}
