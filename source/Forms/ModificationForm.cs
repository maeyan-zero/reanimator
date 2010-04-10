using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reanimator.Excel;
using System.IO;

namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        RevivalMod mod;
        Index[] index;
        TableDataSet dataSet = new TableDataSet();
        
        public ModificationForm()
        {
            InitializeComponent();
            methodComboBox.SelectedIndex = 0;

            OpenIndexFiles();
        }

        private void OpenIndexFiles()
        {
            index = new Index[Index.FileNames.Length];

            for (int i = 0; i < index.Length; i++)
            {
                using (FileStream fs = new FileStream(Config.HglDir + "\\data\\" + Index.FileNames[i] + ".idx", FileMode.Open))
                {
                    index[i] = new Index(fs);
                }
            }
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxDescription.Text = mod.getDescription(checkedListBox.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked)
            {
                mod.setApply(checkedListBox.Items.Count - 1, true);
            }
            else
            {
                mod.setApply(checkedListBox.Items.Count - 1, false);
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            ProgressForm progressForm = new ProgressForm(mod.ModifyExcelFiles, null);
            progressForm.ShowDialog(this);
            progressForm.Dispose();

            progressForm = new ProgressForm(mod.SaveToDisk, null);
            progressForm.ShowDialog(this);
            progressForm.Dispose();

            mod.Dispose();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Modification Files (*.mod, *.xml)|*.mod;*.xml|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK && (openFileDialog.FileName.EndsWith("mod") || openFileDialog.FileName.EndsWith("xml")))
            {
                if (RevivalMod.Parse(openFileDialog.FileName))
                {
                    if (mod == null)
                    {
                        mod = new RevivalMod(openFileDialog.FileName);
                    }
                    else
                    {
                        mod.Append(new RevivalMod(openFileDialog.FileName));
                    }
                    checkedListBox.Items.Clear();
                    for (int i = 0; i < mod.Length; i++)
                    {
                        checkedListBox.Items.Add(mod.getTitle(i), mod.getEnabled(i));
                        if (mod.getEnabled(i))
                        {
                            checkedListBox.SetItemCheckState(i, CheckState.Indeterminate);
                        }
                    }
                    checkedListBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Invalid mod. Check syntax and try again.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
