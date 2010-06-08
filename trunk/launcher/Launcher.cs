using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Reanimator;
using launcher.Properties;
using System.Diagnostics;
using Reanimator.Forms.ItemTransfer;
using launcher.Revival;

namespace launcher
{
    public partial class Launcher : Form
    {
        readonly String _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";
        readonly List<String> _availableCharacters;


        const String HOMEPAGE = "http://www.hellgateaus.net";

        public Launcher()
        {
            _availableCharacters = new List<string>();
            InitializeComponent();
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Config.HglDir))
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

            if (result != DialogResult.Yes) return;

            bool changeMade = false;

            try
            {
                for (int i = 0; i < Index.FileNames.Length; i++)
                {
                    String filePath = String.Format("{0}\\data\\{1}.idx", Config.HglDir, Index.FileNames[i]);
                    if (!File.Exists(filePath))
                    {
                        MessageBox.Show("Index file not found!\n" + filePath, "Warning", MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                        continue;
                    }

                    Index index;
                    try
                    {
                        index = new Index(filePath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load index file!\n\n" + ex, "Error", MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        continue;
                    }

                    if (index.Modified)
                    {
                        if (index.Restore() == false)
                        {
                            throw new Exception("Problem cleaning file: " + Index.FileNames[i]);
                        }
                        changeMade = true;
                    }
                    index.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (changeMade)
            {
                MessageBox.Show("All modifications have been successfully removed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The installation already appears clean.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
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
                            "Artwork by lexsoOr, Music by ..." + Environment.NewLine +
                            Environment.NewLine +
                            "Visit us at " + HOMEPAGE + " " + Environment.NewLine +
                            "Contact maeyan.zero@gmail.com for info.",
                            "HellgateAus.net Launcher 2038", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            _availableCharacters.Add("Start game");
            _availableCharacters.AddRange(Directory.GetFiles(_characterFolder, "*.hg1"));

            //remove path and file extension to get the pure character name
            for (int i = 0; i < _availableCharacters.Count; i++)
            {
                _availableCharacters[i] = _availableCharacters[i].Replace(_characterFolder + @"\", string.Empty).Replace(".hg1", string.Empty);
            }

            //characterCombo.DataSource = _availableCharacters;
        }

        private void p_start_Click(object sender, EventArgs e)
        {
            MinimizeWindow();
            StartGame();
        }

        private void StartGame()
        {
            try
            {
                //if (characterCombo.SelectedIndex > 0)
                //{
                //    String characterToLoad = _characterFolder + @"\" + characterCombo.SelectedItem + @".hg1";
                //    String arguments = "-singleplayer -load\"" + characterToLoad + "\"";

                //    Process.Start(Config.GameClientPath, arguments);
                //}
                //else
                {
                    Process.Start(Config.GameClientPath, "-singleplayer\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to start game at:\n" + Config.GameClientPath + "\n\n" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void p_homePageLink_Click(object sender, EventArgs e)
        {
            MinimizeWindow();
            Process.Start(HOMEPAGE);
        }

        private void MinimizeWindow()
        {
            //minimizes the window to prevent multiple clicks on the launch/openHomePage button
            WindowState = FormWindowState.Minimized;
        }

        private void p_openHomePage_MouseEnter(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Visit it at " + HOMEPAGE;
        }

        private void p_openHomePage_MouseLeave(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void enableHCCharacterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hardcore hardcore = new Hardcore();
            hardcore.ShowDialog();
        }

        private void itemTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SimpleItemTransferForm transfer = new SimpleItemTransferForm();
            transfer.ShowDialog(this);
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            unleshHell.ForeColor = System.Drawing.Color.DarkOrange;
        }

        private void unleshHell_MouseLeave(object sender, EventArgs e)
        {
            unleshHell.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
        }
    }
}
