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
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            hellgateTextBox.Text = Config.HellgatePath;
            clientTextBox.Text = Config.DefaultClientPath;
            saveTextBox.Text = Config.CharacterSavePath;
            preferedDataTypeComboBox.SelectedIndex = Config.PreferredDataType;
            indexStartupCheckBox.Checked = Config.LoadIndexFilesOnStartup;
            excelStartupCheckbox.Checked = Config.LoadExcelFilesOnStartup;
            relationsCheckbox.Checked = Config.EnableExcelRelations;
        }

        private void PathBrowseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrower = new FolderBrowserDialog();
            if (folderBrower.ShowDialog() == DialogResult.OK)
            {
                if ((Button)sender == hellgateBrowseButton)
                {
                    hellgateTextBox.Text = folderBrower.SelectedPath;
                    return;
                }
                if ((Button)sender == saveBrowseButton)
                {
                    saveTextBox.Text = folderBrower.SelectedPath;
                    return;
                }
            }
        }

        private void FileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((Button)sender == clientBrowseButton)
                {
                    clientTextBox.Text = fileDialog.FileName;
                    return;
                }
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            Config.HellgatePath = hellgateTextBox.Text;
            Config.DefaultClientPath = clientTextBox.Text;
            Config.CharacterSavePath = saveTextBox.Text;
            Config.PreferredDataType = preferedDataTypeComboBox.SelectedIndex;
            Config.LoadIndexFilesOnStartup = indexStartupCheckBox.Checked;
            Config.LoadExcelFilesOnStartup = excelStartupCheckbox.Checked;
            Config.EnableExcelRelations = relationsCheckbox.Checked;
            this.Close();
        }
    }
}