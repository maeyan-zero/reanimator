using System;
using System.Windows.Forms;

namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        Modification revival;
        
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
            int i = checkedListBox.SelectedIndex;

            pictureBox.Image = revival.content[i].png;
            titleLabel.Text = revival.content[i].title;
            authorLabel.Text = revival.content[i].author;
            versionLabel.Text = revival.content[i].version;
            urlLabel.Text = revival.content[i].url;
            typeLabel.Text = revival.content[i].type;
            descriptionText.Text = revival.content[i].description;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked ||
                e.CurrentValue == CheckState.Indeterminate)
            {
                revival.content[e.Index].apply = false;
            }
            else
            {
                revival.content[e.Index].apply = true;
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            ProgressForm progressForm = new ProgressForm(revival.Apply, null);
            progressForm.ShowDialog(this);
            progressForm.Dispose();

            progressForm = new ProgressForm(revival.Save, null);
            progressForm.ShowDialog(this);
            progressForm.Dispose();

            revival.Dispose();
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

            if (Modification.Parse(openFileDialog.FileName))
            {
                if (revival == null)
                    revival = new Modification(openFileDialog.FileName);
                else
                    revival.Add(openFileDialog.FileName);

                checkedListBox.Items.Clear();

                for (int i = 0; i < revival.content.Length; i++)
                {
                    checkedListBox.Items.Add(revival.content[i].title, revival.content[i].apply);

                    if (revival.content[i].type == "required")
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Indeterminate);
                        revival.content[i].apply = true;
                    }
                    if (revival.content[i].type == "recommended")
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Checked);
                        revival.content[i].apply = true;
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
            revival.Dispose();
        }
    }
}
