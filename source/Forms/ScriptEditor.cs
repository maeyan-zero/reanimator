using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Reanimator.Forms
{
    public partial class ScriptEditor : Form
    {
        UTF8Encoding _encoding; //used for string to byte[]
        TableDataSet _tableDataSet;
        List<Modification.Pack> _pack;
        Modification.Script _script;

        public string ScriptDir { get { return Config.ScriptDir; } }
        public string Filter { get { return "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*"; } }

        public ScriptEditor(TableDataSet tableDataSet)
        {
            InitializeComponent();
            _encoding = new System.Text.UTF8Encoding();
            _tableDataSet = tableDataSet;
            _pack = new List<Modification.Pack>();

            string[] packs = Directory.GetDirectories(Config.ScriptDir).Where(subDir => (!subDir.Contains("."))).ToArray();

            foreach (string packPath in packs)
            {
                _pack.Add(new Modification.Pack(packPath));
            }

            SetTabs(textBox);
        }

        private void UpdateTreeView()
        {
            treeView.BeginUpdate();
            for (int i = 0; i < _pack.Count; i++)
            {
                treeView.Nodes.Add(_pack[i].Title);
                for (int j = 0; j < _pack[i].Packages.Count; j++)
                {
                    treeView.Nodes[i].Nodes.Add(_pack[i].Packages[j].Title);
                    for (int k = 0; k < _pack[i].Packages[j].Scripts.Count; k++)
                    {
                        treeView.Nodes[i].Nodes[j].Nodes.Add(_pack[i].Packages[j].Scripts[k].Title);
                    }
                }
                treeView.Nodes[i].Nodes.Add("config.ini");
            }
            treeView.EndUpdate();
        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Config.HglDir) && !Directory.Exists(ScriptDir))
            {
                Directory.CreateDirectory(ScriptDir);
            }
            else
            {
                UpdateTreeView();
            }
            Reanimator form = (Reanimator)MdiParent;
            form.MainMenuStrip.Items["scriptsMenu"].Visible = true;
        }

        private void availableCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            savePrompt();
            //if (_currentTable != availableCombo.SelectedIndex)
            //{
            //    int i = availableCombo.SelectedIndex;
            //    using (FileStream fileStream = new FileStream(@_availableFiles[i], FileMode.Open))
            //    {
            //        byte[] buffer = new byte[fileStream.Length];
            //        fileStream.Read(buffer, 0, (int)fileStream.Length);
            //        textBox.Text = _encoding.GetString(buffer);
            //        _currentTable = i;
            //    }
            //}
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            byte[] buffer = _encoding.GetBytes(textBox.Text);

            //if (_currentTable == -1)
            //{
            //    SaveFileDialog saveFileDialogue = new SaveFileDialog();
            //    saveFileDialogue.InitialDirectory = ScriptDir;
            //    saveFileDialogue.Filter = Filter;
            //    saveFileDialogue.ShowDialog();

            //    //write new file stream if user has choosen a filename
            //    if (saveFileDialogue.FileName != "")
            //    {
            //        using (FileStream fileStream = new FileStream(@saveFileDialogue.FileName, FileMode.Create))
            //        {
            //            fileStream.Write(buffer, 0, buffer.Length);
            //        }

            //        textBox.Modified = false;
            //        string filename = PathTools.FileNameFromPath(saveFileDialogue.FileName);
            //        availableCombo.Items.Add(filename);
            //        _availableFiles.Add(saveFileDialogue.FileName);
            //        _currentTable = availableCombo.Items.Count - 1;
            //        availableCombo.SelectedIndex = _currentTable;
            //    }
            //}
            //else
            //{
            //    using (FileStream fileStream = new FileStream(@_availableFiles[_currentTable], FileMode.Create))
            //    {
            //        fileStream.Write(buffer, 0, buffer.Length);
            //        textBox.Modified = false;
            //    }
            //}
        }

        private void newButton_Click(object sender, EventArgs e)
        {

        }

        private bool savePrompt()
        {
            if (textBox.Modified)
            {
                if (_script == null) // not saved yet
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
                else // save changes?
                {
                    DialogResult result = MessageBox.Show("Do you want to save the changes?",
                        "Changes have been made.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        byte[] buffer = _encoding.GetBytes(textBox.Text);

                        using (FileStream fileStream = new FileStream(@_script.Path, FileMode.Create))
                        {
                            fileStream.Write(buffer, 0, buffer.Length);
                        }
                        return true;
                    }
                }
            }
            else
            {
                return true; // wasn't modified, okay
            }
            return false; // its modified but not saved
        }

        private void apply(int i)
        {

            //Modification modification = new Modification(_tableDataSet);
            //string path = _availableFiles[i];
            //ProgressForm progress = new ProgressForm(modification.Open, path);
            //progress.ShowDialog();
            //progress = new ProgressForm(modification.Apply, modification);
            //progress.ShowDialog();
        }

        private void applyCurrentButton_Click(object sender, EventArgs e)
        {
            bool okay = savePrompt();
            //if (okay) apply(availableCombo.SelectedIndex);
        }

        private void applyCheckedButton_Click(object sender, EventArgs e)
        {
            //foreach (int i in availableCombo.CheckedIndices)
            //{
            //    bool okay = savePrompt();
            //    if (okay) apply(i);
            //}
        }

        private void ScriptEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            savePrompt();
            Reanimator form = (Reanimator)MdiParent;
            form.MainMenuStrip.Items["scriptsMenu"].Visible = false;
        }

        static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, ref int lParam);
        }

        static void SetTabs(TextBox box)
        {
            //EM_SETTABSTOPS - http://msdn.microsoft.com/en-us/library/bb761663%28VS.85%29.aspx
            int lParam = 16;  //Set tab size to 4 spaces
            NativeMethods.SendMessage(box.Handle, 0x00CB, new IntPtr(1), ref lParam);
            box.Invalidate();
        }

        private void newGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            savePrompt();

            if (e.Node.Level == 2)
            {
                int script = e.Node.Index;
                int package = e.Node.Parent.Index;
                int group = e.Node.Parent.Parent.Index;
                string path = _pack[group].Packages[package].Scripts[script].Path;

                using (FileStream fileStream = new FileStream(@path, FileMode.Open))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    textBox.Text = _encoding.GetString(buffer);
                    _script = _pack[group].Packages[package].Scripts[script];
                }
            }
            else if (e.Node.Level == 1 && e.Node.Text == "config.ini")
            {
                int group = e.Node.Parent.Index;
                string path = _pack[group].Path + "\\config.ini";

                using (FileStream fileStream = new FileStream(@path, FileMode.Open))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, (int)fileStream.Length);
                    textBox.Text = _encoding.GetString(buffer);
                }
            }
        }
    }
}
