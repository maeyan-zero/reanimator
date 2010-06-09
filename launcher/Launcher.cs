using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Reanimator;
using launcher.Properties;
using System.Diagnostics;
using Reanimator.Forms;
using Reanimator.Forms.ItemTransfer;
using launcher.Revival;

namespace launcher
{
    public partial class Launcher : Form
    {
        readonly String _characterFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\My Games\\Hellgate\\Save\\Singleplayer";
        readonly List<String> _availableCharacters;
        ModificationForm modForm;

        const String HOMEPAGE = "http://www.hellgateaus.net";

        public Launcher()
        {
            _availableCharacters = new List<string>();
            InitializeComponent();
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Config.HglDir))
                modForm = new ModificationForm();
            else
                MessageBox.Show("Can't locate Hellgate: London directory. Check settings and try again.");
        }

        private void revertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.IndexBackupCreated)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to revert all modifications?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                foreach (String f in Directory.GetFiles(Config.HglDir + "\\data\\backup"))
                {
                    int i = f.LastIndexOf("\\");
                    String filename = f.Substring(i, f.Length - i);
                    File.Copy(f, Config.HglDir + "\\data" + filename, true);
                }

                MessageBox.Show("Modifications uninstalled.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No backup state found. You must first install modifications.", "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            "Artwork by ArtmanPhil." + Environment.NewLine +
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
            unleshHell.ForeColor = System.Drawing.Color.White;
        }

        private void unleshHell_MouseLeave(object sender, EventArgs e)
        {
            unleshHell.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
        }

        private void itemTransferToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //ComplexItemTransferForm form = new ComplexItemTransferForm();
            //form.ShowDialog(this);
        }
    }
}
