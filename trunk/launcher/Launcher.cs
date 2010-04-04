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
using launcher.Properties;
using System.Diagnostics;

namespace launcher
{
    public partial class Launcher : Form
    {
        string indexPath;
        String[] saveFolderContents;
        int[] characterIndex;

        public Launcher()
        {
            indexPath = Config.DataDirsRoot + "\\data\\" + Mod.defaultPack + ".idx";
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

                if (index.IsModified())
                {
                    if (index.Restore(indexPath) == true)
                    {
                        MessageBox.Show("All modifications successfully removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("There was a problem retoring the index.", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Installation already appears clean.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                stream.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reanimator.Forms.Options options = new Reanimator.Forms.Options();
            options.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("HellgateAus.net Launcher 2038" + Environment.NewLine +
                            "Developed by Maeyan, Alex2069, Kite & Malachor." + Environment.NewLine +
                            "Visit us at http://www.hellgateaus.net" + Environment.NewLine +
                            "Contact maeyan.zero@gmail.com for info.",
                            "HellgateAus.net Launcher 2038", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            String characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";
            saveFolderContents = Directory.GetFiles(characterFolder);
            characterIndex = new int[saveFolderContents.Length + 1];
            
            characterCombo.Items.Add("");
            for (int i = 0; i < saveFolderContents.Length; i++)
            {
                if (saveFolderContents[i].Contains(".hg1"))
                {
                    String characterName = saveFolderContents[i].Remove(0, saveFolderContents[i].LastIndexOf("\\") + 1);
                    characterName = characterName.Remove(characterName.Length - 4, 4);
                    characterCombo.Items.Add(characterName);
                    characterIndex[characterCombo.Items.Count - 1] = i;
                }
            }
        }

        private void p_start_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            try
            {
                if (characterCombo.SelectedIndex != -1 && characterCombo.SelectedIndex != 0)
                {
                    System.Diagnostics.Process.Start(Config.GameClientPath, "-singleplayer -load\"" + saveFolderContents[characterIndex[characterCombo.SelectedIndex]] + "\"");
                }
                else
                {
                    System.Diagnostics.Process.Start(Config.GameClientPath, "-singleplayer\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game at:\n" + Config.GameClientPath + "\n\n" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void p_start_MouseEnter(object sender, EventArgs e)
        {
            p_start.BackgroundImage = Resources.templar_mouseOver;
        }

        private void p_start_MouseLeave(object sender, EventArgs e)
        {
            p_start.BackgroundImage = Resources.templar_normal;
        }

        private void p_start_MouseDown(object sender, MouseEventArgs e)
        {
            p_start.BackgroundImage = Resources.templar_normal;
        }

        private void p_start_MouseUp(object sender, MouseEventArgs e)
        {
            p_start.BackgroundImage = Resources.templar_mouseOver;
        }

        private void p_homePageLink_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://www.hellgateaus.net");
        }
    }
}
