﻿using System;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        RevivalMod mod;
        
        public ModificationForm()
        {
            InitializeComponent();
            titleLabel.Text = "";
            authorLabel.Text = "";
            versionLabel.Text = "";
            urlLabel.Text = "";
            typeLabel.Text = "";
        }

        private void checkedListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox.Image = mod.getCaption(checkedListBox.SelectedIndex);
            titleLabel.Text = mod.GetTitle(checkedListBox.SelectedIndex);
            authorLabel.Text = mod.GetAuthor(checkedListBox.SelectedIndex);
            versionLabel.Text = mod.GetVersion(checkedListBox.SelectedIndex);
            urlLabel.Text = mod.GetUrl(checkedListBox.SelectedIndex);
            typeLabel.Text = mod.GetUsage(checkedListBox.SelectedIndex);
            descriptionText.Text = mod.GetDescription(checkedListBox.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked)
            {
                mod.SetApply(checkedListBox.Items.Count - 1, true);
            }
            else
            {
                mod.SetApply(checkedListBox.Items.Count - 1, false);
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
            OpenFileDialog openFileDialog = new OpenFileDialog
                                                {
                                                    Filter =
                                                        "Modification Files (*.zip, *.xml)|*.zip;*.xml|All Files (*.*)|*.*"
                                                };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK ||
                (!openFileDialog.FileName.EndsWith("xml") &&
                    !openFileDialog.FileName.EndsWith("zip"))) return;

            if (openFileDialog.FileName.EndsWith("zip"))
            {
                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(openFileDialog.FileName))
                {
                    zip.ExtractAll(Config.HglDir + "\\modpacks\\", Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
                }
                
                int lastof1 = openFileDialog.FileName.LastIndexOf("\\");
                int lastof2 = openFileDialog.FileName.LastIndexOf(".");
                int length = lastof2 - lastof1;
                string modname = openFileDialog.FileName.Substring(lastof1, length);

                openFileDialog.FileName = Config.HglDir + "\\modpacks" + modname + "\\mod.xml";
                System.IO.Directory.SetCurrentDirectory(Config.HglDir + "\\modpacks" + modname);
            }

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
                    checkedListBox.Items.Add(mod.GetTitle(i), mod.GetEnabled(i));
                    mod.SetApply(i, mod.GetEnabled(i));
                    if (mod.GetUsage(i) == "required")
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Indeterminate);
                    }
                    if (mod.GetUsage(i) == "recommended")
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Checked);
                    }
                }
                checkedListBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Invalid mod. Check syntax and try again.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public new void Close()
        {
            mod.Dispose();
            Dispose();
        }
    }
}
