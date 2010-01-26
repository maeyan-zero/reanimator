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
                hglDir.Text = Config.hglDir;

                if (Config.dataDirsRootChecked)
                {
                    Config.dataDirsRoot = folderBrowserDialogue.SelectedPath;
                }
            }
        }

        private void dataDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Config.dataDirsRootChecked = dataDirCheckBox.Checked;
            RefreshDataDir();
        }

        private void RefreshDataDir()
        {
            if (dataDirCheckBox.Checked)
            {
                Config.dataDirsRoot = Config.hglDir;
            }

            dataDirTextBox.Text = Config.dataDirsRoot;
            dataDirBrowse.Enabled = !dataDirCheckBox.Checked;
            dataDirTextBox.Enabled = !dataDirCheckBox.Checked;
        }

        private void dataDirBrowse_Click(object sender, EventArgs e)
        {
            if (!dataDirBrowse.Enabled)
            {
                return;
            }

            FolderBrowserDialog folderBrowserDialogue = new FolderBrowserDialog();
            folderBrowserDialogue.Description = "Locate the Hellgate: London installation directory. Example: C:\\Program Files\\Flagship Studios\\Hellgate London";
            folderBrowserDialogue.SelectedPath = Config.hglDir;

            if (folderBrowserDialogue.ShowDialog(this) == DialogResult.OK)
            {
                Config.dataDirsRoot = folderBrowserDialogue.SelectedPath;
                dataDirTextBox.Text = Config.dataDirsRoot;
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir.Text = Config.hglDir;
            dataDirTextBox.Text = Config.dataDirsRoot;
            dataDirCheckBox.Checked = Config.dataDirsRootChecked;
            dataDirBrowse.Enabled = !Config.dataDirsRootChecked;
            dataDirTextBox.Enabled = !Config.dataDirsRootChecked;
        }
    }
}
