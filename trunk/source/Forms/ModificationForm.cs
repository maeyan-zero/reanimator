using System;
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
            titleLabel.Text = mod.getTitle(checkedListBox.SelectedIndex);
            authorLabel.Text = mod.getAuthor(checkedListBox.SelectedIndex);
            versionLabel.Text = mod.getVersion(checkedListBox.SelectedIndex);
            urlLabel.Text = mod.getUrl(checkedListBox.SelectedIndex);
            typeLabel.Text = mod.getUsage(checkedListBox.SelectedIndex);
            descriptionText.Text = mod.getDescription(checkedListBox.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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
            OpenFileDialog openFileDialog = new OpenFileDialog
                                                {
                                                    Filter =
                                                        "Modification Files (*.mod, *.xml)|*.mod;*.xml|All Files (*.*)|*.*"
                                                };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK ||
                (!openFileDialog.FileName.EndsWith("mod") && !openFileDialog.FileName.EndsWith("xml"))) return;

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
                    mod.setApply(i, mod.getEnabled(i));
                    if (mod.getUsage(i) == "required")
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Indeterminate);
                    }
                    if (mod.getUsage(i) == "recommended")
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
    }
}
