using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Launcher.Forms
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            hellgateTextBox.Text = Config.HglDir;
            clientTextBox.Text = Config.GameClientPath;
            saveTextBox.Text = Config.SaveDir;
            backupTextBox.Text = Config.BackupDir;
        }

        private void PathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrower = new FolderBrowserDialog();
            if (folderBrower.ShowDialog() == DialogResult.OK)
            {
                if ((Button)sender == hellgateButton)
                {
                    hellgateTextBox.Text = folderBrower.SelectedPath;
                    return;
                }
                if ((Button)sender == saveButton)
                {
                    saveTextBox.Text = folderBrower.SelectedPath;
                    return;
                }
                if ((Button)sender == backupButton)
                {
                    backupTextBox.Text = folderBrower.SelectedPath;
                    return;
                }
            }
        }

        private void FileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((Button)sender == clientButton)
                {
                    clientTextBox.Text = fileDialog.FileName;
                    return;
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Config.HglDir = hellgateTextBox.Text;
            Config.GameClientPath = clientTextBox.Text;
            Config.SaveDir = saveTextBox.Text;
            Config.BackupDir = backupTextBox.Text;
            this.Close();
        }
    }
}
