using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Revival.Common;
using Hellgate;

namespace Reanimator.Forms
{
    public partial class Options : Form
    {
        private readonly FileManager _fileManager;

        public Options(FileManager fileManager)
        {
            _fileManager = fileManager;

            InitializeComponent();
        }

        private void _OkButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void _HglDirBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
            {
                Description = "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London",
                SelectedPath = Config.HglDir
            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            Config.HglDir = folderBrowserDialogue.SelectedPath;
            hglDir_TextBox.Text = Config.HglDir;
            _UpdateStringsLanguages();
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir_TextBox.Text = Config.HglDir;
            gameClientPath_TextBox.Text = Config.GameClientPath;
            scriptDirText.Text = Config.ScriptDir;
            intPtrTypeCombo.SelectedItem = Config.IntPtrCast;
            relationsCheck.Checked = Config.GenerateRelations;
            txtEditor_TextBox.Text = Config.TxtEditor;
            xmlEditor_TextBox.Text = Config.XmlEditor;
            csvEditor_TextBox.Text = Config.CsvEditor;
            _tcv4_CheckBox.Checked = Config.LoadTCv4DataFiles;
            _UpdateStringsLanguages();
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

        private void _GameClientPath_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*",
                InitialDirectory = Config.HglDir + "\\SP_x64"
            };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;

            Config.GameClientPath = openFileDialog.FileName;
            gameClientPath_TextBox.Text = openFileDialog.FileName;
        }

        private void _ScriptButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
            {
                Description = "Locate the Reanimator script directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London\\Reanimator\\Scripts",
                SelectedPath = Config.ScriptDir
            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            Config.ScriptDir = folderBrowserDialogue.SelectedPath;
            scriptDirText.Text = Config.ScriptDir;
        }

        private void _IntPtrTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.IntPtrCast = intPtrTypeCombo.Text;
        }

        private void _RelationsCheck_CheckedChanged(object sender, EventArgs e)
        {
            Config.GenerateRelations = relationsCheck.Checked;
        }

        private void _TxtEditor_Button_Click(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            Config.TxtEditor = filePath;
            txtEditor_TextBox.Text = Config.TxtEditor;
        }

        private void _XmlEditor_Button_Click(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            Config.XmlEditor = filePath;
            xmlEditor_TextBox.Text = Config.XmlEditor;
        }

        private void _CsvEditor_Button_Click(object sender, EventArgs e)
        {
            String filePath = FormTools.OpenFileDialogBox("exe", "Executable", Path.GetDirectoryName(Config.TxtEditor));
            if (filePath == null) return;

            Config.CsvEditor = filePath;
            csvEditor_TextBox.Text = Config.CsvEditor;
        }

        private void _TCv4_CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.LoadTCv4DataFiles = _tcv4_CheckBox.Checked;
        }

        private void _StringsLang_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.StringsLanguage = (String)_stringsLang_comboBox.SelectedItem;
        }
    }
}