using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog();
            folderBrowserDialogue.Description = "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London";
            folderBrowserDialogue.SelectedPath = Config.hglDir;

            if (folderBrowserDialogue.ShowDialog(this) == DialogResult.OK)
            {
                Config.hglDir = folderBrowserDialogue.SelectedPath;
                hglDir_TextBox.Text = Config.hglDir;

                if (Config.dataDirsRootChecked)
                {
                    Config.dataDirsRoot = folderBrowserDialogue.SelectedPath;
                }
            }
        }

        private void dataDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.dataDirsRootChecked = dataDir_CheckBox.Checked;
            RefreshDataDir();
        }

        private void RefreshDataDir()
        {
            if (dataDir_CheckBox.Checked)
            {
                Config.dataDirsRoot = Config.hglDir;
            }

            dataDir_TextBox.Text = Config.dataDirsRoot;
            dataDir_Button.Enabled = !dataDir_CheckBox.Checked;
            dataDir_TextBox.Enabled = !dataDir_CheckBox.Checked;
        }

        private void dataDir_Button_Clicked(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog();
            folderBrowserDialogue.Description = "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London";
            folderBrowserDialogue.SelectedPath = Config.hglDir;

            if (folderBrowserDialogue.ShowDialog(this) == DialogResult.OK)
            {
                Config.dataDirsRoot = folderBrowserDialogue.SelectedPath;
                dataDir_TextBox.Text = Config.dataDirsRoot;
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir_TextBox.Text = Config.hglDir;
            dataDir_TextBox.Text = Config.dataDirsRoot;
            dataDir_CheckBox.Checked = Config.dataDirsRootChecked;
            dataDir_Button.Enabled = !Config.dataDirsRootChecked;
            dataDir_TextBox.Enabled = !Config.dataDirsRootChecked;
            gameClientPath_TextBox.Text = Config.gameClientPath;
        }

        private void gameClientPath_Button_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = Config.hglDir + "\\SP_x64";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Config.gameClientPath = openFileDialog.FileName;
                gameClientPath_TextBox.Text = openFileDialog.FileName;
            }
        }
    }
}
