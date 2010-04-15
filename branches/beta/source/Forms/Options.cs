using System;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void hglDirBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
                                                            {
                                                                Description =
                                                                    "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London",
                                                                SelectedPath = Config.HglDir
                                                            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            Config.HglDir = folderBrowserDialogue.SelectedPath;
            hglDir_TextBox.Text = Config.HglDir;

            if (Config.DataDirsRootChecked)
            {
                Config.DataDirsRoot = folderBrowserDialogue.SelectedPath;
            }
        }

        private void dataDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.DataDirsRootChecked = dataDir_CheckBox.Checked;
            RefreshDataDir();
        }

        private void RefreshDataDir()
        {
            if (dataDir_CheckBox.Checked)
            {
                Config.DataDirsRoot = Config.HglDir;
            }

            dataDir_TextBox.Text = Config.DataDirsRoot;
            dataDir_Button.Enabled = !dataDir_CheckBox.Checked;
            dataDir_TextBox.Enabled = !dataDir_CheckBox.Checked;
        }

        private void dataDir_Button_Clicked(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
                                                            {
                                                                Description =
                                                                    "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London",
                                                                SelectedPath = Config.HglDir
                                                            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            Config.DataDirsRoot = folderBrowserDialogue.SelectedPath;
            dataDir_TextBox.Text = Config.DataDirsRoot;
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir_TextBox.Text = Config.HglDir;
            dataDir_TextBox.Text = Config.DataDirsRoot;
            dataDir_CheckBox.Checked = Config.DataDirsRootChecked;
            dataDir_Button.Enabled = !Config.DataDirsRootChecked;
            dataDir_TextBox.Enabled = !Config.DataDirsRootChecked;
            gameClientPath_TextBox.Text = Config.GameClientPath;
        }

        private void gameClientPath_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
                                                {
                                                    Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*",
                                                    InitialDirectory = Config.HglDir + "\\SP_x64"
                                                };

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Config.GameClientPath = openFileDialog.FileName;
                gameClientPath_TextBox.Text = openFileDialog.FileName;
            }
        }
    }
}