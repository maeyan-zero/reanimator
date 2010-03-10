using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Reanimator;

namespace launcher
{
    public partial class Launcher : Form
    {
        string indexPath;

        public Launcher()
        {
            indexPath = Config.DataDirsRoot + "\\" + Mod.defaultPack + ".idx";
            InitializeComponent();
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Config.HglDir) == true)
            {
                Reanimator.Forms.ModificationForm modForm = new Reanimator.Forms.ModificationForm();
                modForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Can't locate Hellgate: London directory. Check settings and try again.");
            }
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to revert all modifications?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                FileStream stream = new FileStream(@indexPath, FileMode.Open);
                Index index = new Index(stream);

                if (Utility.IndexIsModified(index))
                {
                    if (Utility.RestoreIndex(index, indexPath) == true)
                    {
                        MessageBox.Show("All modifications successfully removed.", "Success", MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("There was a problem retoring the index.", "Failure", MessageBoxButtons.OK);
                    }
                }
                else
                {
                    MessageBox.Show("Installation already appears clean.", "Information", MessageBoxButtons.OK);
                }

                stream.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
