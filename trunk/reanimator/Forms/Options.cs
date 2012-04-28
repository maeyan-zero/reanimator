using System;
using System.IO;
using System.Windows.Forms;
using Revival.Common;
using Hellgate;
using System.Drawing;

namespace Reanimator.Forms
{
    public partial class Options : Form
    {
        private readonly FileManager _fileManager;
        private bool _needRestart; // persistent need-restart flag

        public Options(FileManager fileManager)
        {
            _fileManager = fileManager;

            InitializeComponent();
        }

        private void _OptionsOnLoad(object sender, EventArgs e)
        {
            _LoadOptions();
        }

        private void _LoadOptions()
        {
            // General - Path Settings
            hglDir_TextBox.Text = Config.HglDir; // restart required
            gameClientPath_TextBox.Text = Config.GameClientPath;
            scriptDirText.Text = Config.ScriptDir;

            // General - Load Options
            _tcv4_CheckBox.Checked = Config.LoadTCv4DataFiles; // restart required

            // General - Language Files
            _UpdateStringsLanguages(); // restart required

            // Display - Xls Editor
            intPtrTypeCombo.SelectedItem = Config.IntPtrCast;
            relationsCheck.Checked = Config.GenerateRelations;

            // Default Programs
            txtEditor_TextBox.Text = Config.TxtEditor;
            xmlEditor_TextBox.Text = Config.XmlEditor;
            csvEditor_TextBox.Text = Config.CsvEditor;
        }

        private void _OkButtonClick(object sender, EventArgs e)
        {
            bool optionsSaved = _SaveOptions(); // apply changes and check if want/need to restart
            if (!optionsSaved) return; // hit cancel

            Hide();
        }

        private void _CancelButtonClick(object sender, EventArgs e)
        {
            _LoadOptions(); // undo changes
            Hide();
        }

        private bool _SaveOptions()
        {
            // have the settings changed - will we need to restart
            if (Config.HglDir != hglDir_TextBox.Text ||
                Config.LoadTCv4DataFiles != _tcv4_CheckBox.Checked ||
                (_stringsLang_comboBox.SelectedItem != null && Config.StringsLanguage != (String)_stringsLang_comboBox.SelectedItem))
            {
                _needRestart = true;
            }

            // don't do this above - want this here for persistence
            bool doRestart = false;
            if (_needRestart)
            {
                DialogResult drRestart = MessageBox.Show("Some options require a restart to take effect.\nRestart now?", "Restart Needed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (drRestart == DialogResult.Cancel)
                {
                    _needRestart = false;
                    return false;
                }

                doRestart = (drRestart == DialogResult.Yes);
            }

            // apply changes
            // path options
            Config.HglDir = hglDir_TextBox.Text; // restart required
            Config.GameClientPath = gameClientPath_TextBox.Text;
            Config.ScriptDir = scriptDirText.Text;

            // load options
            Config.LoadTCv4DataFiles = _tcv4_CheckBox.Checked; // restart required

            // editor options
            Config.TxtEditor = txtEditor_TextBox.Text;
            Config.XmlEditor = xmlEditor_TextBox.Text;
            Config.CsvEditor = csvEditor_TextBox.Text;

            // xls display options
            Config.IntPtrCast = intPtrTypeCombo.Text;
            Config.GenerateRelations = relationsCheck.Checked;

            // language options
            if (_stringsLang_comboBox.SelectedItem != null) // only update if valid (can happen when changing HglDir stuff etc.)
            {
                Config.StringsLanguage = (String) _stringsLang_comboBox.SelectedItem; // restart required
                _UpdateStringsLanguages();
            }

            // restart if authorized
            if (doRestart) Application.Restart();

            return true;
        }

        private void _HglDirBrowseClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
            {
                Description = "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London",
                SelectedPath = Config.HglDir
            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            hglDir_TextBox.Text = folderBrowserDialogue.SelectedPath;
        }

        private void _UpdateStringsLanguages()
        {
            _stringsLang_comboBox.Items.Clear();

            String[] directories = _fileManager.GetLanguages();
            //_fileManager.FileEntries.Values.Where(file => file.Directory.Contains());
            if (directories == null) return;

            _stringsLang_comboBox.Items.Add(String.Empty); // needed if current language isn't set, or isn't found in currently chosen HG path (e.g. Resurrection clients without English)
            foreach (String stringsDir in directories)
            {
                _stringsLang_comboBox.Items.Add(stringsDir);
            }

            _stringsLang_comboBox.SelectedItem = Config.StringsLanguage;
        }

        private void _GameClientPathButtonClick(object sender, EventArgs e)
        {
            String initialDir = Config.HglDir + "\\SP_x64";
            if (!Directory.Exists(initialDir)) initialDir = Config.HglDir;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*",
                InitialDirectory = Config.HglDir + "\\SP_x64"
            };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            gameClientPath_TextBox.Text = openFileDialog.FileName;
        }

        private void _ScriptButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
            {
                Description = "Locate the Reanimator script directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London\\Reanimator\\Scripts",
                SelectedPath = Config.ScriptDir
            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            scriptDirText.Text = folderBrowserDialogue.SelectedPath;
        }

        private void _TxtEditorButtonClick(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            txtEditor_TextBox.Text = filePath;
        }

        private void _XmlEditorButtonClick(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            xmlEditor_TextBox.Text = filePath;
        }

        private void _CsvEditorButtonClick(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            csvEditor_TextBox.Text = filePath;
        }

        private void _HglConfigTextChanged(object sender, EventArgs e)
        {
            hglDir_TextBox.ForeColor = Color.Black;
            if (!Directory.Exists(hglDir_TextBox.Text))
            {
                hglDir_TextBox.ForeColor = Color.Red;
            }

            gameClientPath_TextBox.ForeColor = Color.Black;
            if (!File.Exists(gameClientPath_TextBox.Text))
            {
                gameClientPath_TextBox.ForeColor = Color.Red;
            }

            scriptDirText.ForeColor = Color.Black;
            if (!Directory.Exists(scriptDirText.Text))
            {
                scriptDirText.ForeColor = Color.Red;
            }
        }
    }
}