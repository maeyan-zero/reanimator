using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
namespace Reanimator.Forms
{
    public partial class ModificationForm : Form
    {
        TableDataSet _tableDataSet;
        Modification _modification;
        //List<Modification.Package> _package;
        
        public ModificationForm(TableDataSet tableDataset)
        {
            _tableDataSet = tableDataset;
            _modification = new Modification(_tableDataSet);
            InitializeComponent();

            add_Click(this, null); // automatically asks user for a file
        }
        private void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = checkedListBox.SelectedIndex;

            //if (revival.content[i].png == null)
            //{
            //    String path = System.IO.Directory.GetCurrentDirectory() + "\\" + revival.content[i].image;

            //    if (System.IO.File.Exists(path))
            //        revival.content[i].png = Image.FromFile(path);
            //}

            //pictureBox.Image = revival.content[i].png;
            //titleLabel.Text = revival.content[i].title;
            //authorLabel.Text = revival.content[i].author;
            //versionLabel.Text = revival.content[i].version;
            //urlLabel.Text = revival.content[i].url;
            //typeLabel.Text = revival.content[i].type;
            //descriptionText.Text = revival.content[i].description;
        }
        private void itemList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //if (e.CurrentValue == CheckState.Checked ||
            //    e.CurrentValue == CheckState.Indeterminate)
            //{
            //    revival.content[e.Index].apply = false;
            //}
            //else
            //{
            //    revival.content[e.Index].apply = true;
            //}
        }
        private void install_Click(object sender, EventArgs e)
        {
            //if (revival.InstallIsClean)
            //{
            //    if (!Config.IndexBackupCreated)
            //    {
            //        if (!Directory.Exists(Config.HglDir + "\\data\\backup"))
            //            Directory.CreateDirectory(Config.HglDir + "\\data\\backup");
            //        foreach(string f in Directory.GetFiles(Config.HglDir + "\\data")) {
            //            if (f.Contains(".idx")) {
            //                int i = f.LastIndexOf("\\") + 1;
            //                string dest = Config.HglDir + "\\data\\backup\\" + f.Substring(i);
            //                File.Copy(f, dest);
            //            }
            //        }
            //        Config.IndexBackupCreated = true;
            //    }
            //    ProgressForm progressForm = new ProgressForm(revival.Apply, null);
            //    progressForm.ShowDialog(this);
            //    progressForm.Dispose();

            //    progressForm = new ProgressForm(revival.Save, null);
            //    progressForm.ShowDialog(this);
            //    progressForm.Dispose(); ;
            //}
            //else
            //{
            //    MessageBox.Show("Hellgate London is already modifified.\nPlease revert modifications.",
            //        "Error.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
        }
        private void add_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Modification Files (*.zip)|*.zip;|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog(this) != DialogResult.OK) return;
            if (!openFileDialog.FileName.EndsWith("zip")) return;

            //ProgressForm progressForm = new ProgressForm(_modification.Open, openFileDialog.FileName);
            //progressForm.ShowDialog(this);
            //progressForm.Dispose();

            //_package = _modification.ModPackage;

            checkedListBox.Items.Clear();

            //for (int i = 0; i < _package.Count; i++)
            //{
            //    checkedListBox.Items.Add(revival.content[i].title, revival.content[i].apply);

            //    if (revival.content[i].type == "required")
            //    {
            //        checkedListBox.SetItemCheckState(i, CheckState.Indeterminate);
            //        revival.content[i].apply = true;
            //    }
            //    if (revival.content[i].type == "recommended")
            //    {
            //        checkedListBox.SetItemCheckState(i, CheckState.Checked);
            //        revival.content[i].apply = true;
            //    }
            //}
            checkedListBox.SelectedIndex = 0;
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
