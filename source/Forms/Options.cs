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
        Config config;

        public Options(Config c)
        {
            config = c;

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
            folderBrowserDialogue.SelectedPath = config["hglDir"];

            if (folderBrowserDialogue.ShowDialog(this) == DialogResult.OK)
            {
                config["hglDir"] = folderBrowserDialogue.SelectedPath;
                hglDir.Text = config["hglDir"];
            }
        }

        private void dataDirCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            config["dataDirChecked"] = dataDirCheckBox.Checked.ToString();
            RefreshDataDir();
        }

        private void RefreshDataDir()
        {
            if (dataDirCheckBox.Checked)
            {
                config["dataDir"] = config["hglDir"];
            }
            
            dataDirTextBox.Text = config["dataDir"];
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
            folderBrowserDialogue.SelectedPath = config["hglDir"];

            if (folderBrowserDialogue.ShowDialog(this) == DialogResult.OK)
            {
                config["dataDir"] = folderBrowserDialogue.SelectedPath;
                dataDirTextBox.Text = config["dataDir"];
            }
        }

        private void Options_Load(object sender, EventArgs e)
        {
            hglDir.Text = config["hglDir"];
            dataDirTextBox.Text = config["dataDir"];
            dataDirCheckBox.Checked = ((string)(config["dataDirChecked"])).Equals("True");
            dataDirBrowse.Enabled = !dataDirCheckBox.Checked;
            dataDirTextBox.Enabled = !dataDirCheckBox.Checked;
        }
    }
}
