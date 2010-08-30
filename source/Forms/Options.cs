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
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir_TextBox.Text = Config.HglDir;
            gameClientPath_TextBox.Text = Config.GameClientPath;
            scriptDirText.Text = Config.ScriptDir;
            intPtrTypeCombo.SelectedItem = Config.IntPtrCast;
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

        private void scriptButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog
            {
                Description =
                    "Locate the Reanimator script directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London\\Reanimator\\Scripts",
                SelectedPath = Config.ScriptDir
            };

            if (folderBrowserDialogue.ShowDialog(this) != DialogResult.OK) return;

            Config.ScriptDir = folderBrowserDialogue.SelectedPath;
            scriptDirText.Text = Config.ScriptDir;
        }

        private void intPtrTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Config.IntPtrCast = intPtrTypeCombo.Text;
        }
    }
}