using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Reanimator.Forms
{
    public partial class UpdateForm : Form//ThreadedFormBase
    {
        UpdateCheckerParams currentVersion;
        private delegate void UpdateList(NewMod[] mods);
        private delegate void UpdateText(string text);
        UpdateList uList;
        UpdateText uText;

        public UpdateForm(UpdateCheckerParams parameters)
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
            uList = UpdateListBox;
            uText = UpdateTextFields;

            this.currentVersion = parameters;
        }

        private void UpdateListBox(NewMod[] mods)
        {
            lb_availableUpdates.Items.Clear();
            Version tmp = currentVersion.installedVersion.version;

            foreach (NewMod mod in mods)
            {
                if(mod.IsNewestVersion(tmp))
                {
                    this.Text = "New version available!" + currentVersion.installedVersion.version + " -> " + mod.version;
                    tmp = mod.version;
                }
                if(File.Exists(currentVersion.saveFolder + mod.ToString()))
                {
                    mod.alreadyPresent = true;
                }
            }

            if (currentVersion.installedVersion.version == tmp)
            {
                this.Text = "No new versions available!";
            }

            lb_availableUpdates.DataSource = mods;
        }

        private void UpdateTextFields(string text)
        {
            this.Text = text;
        }

        public void GetModInfo()
        {
            try
            {
                UpdateChecker checker = currentVersion.updateChecker;
                checker.GetWebsiteCompleteEvent += new UpdateChecker.GetWebsiteComplete(checker_GetWebsiteCompleteEvent);
                checker.FileDownloadCompleteEvent += new UpdateChecker.FileDownloadComplete(checker_FileDownloadCompleteEvent);

                this.Invoke(uText, "Searching for updates...");
                checker.GetWebsiteByUrl(currentVersion.installedVersion.link);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void checker_GetWebsiteCompleteEvent(List<NewMod> mods)
        {
            try
            {
                this.Invoke(uText, mods.Count + " versions found.");
                this.Invoke(uList, (object)mods.ToArray());

                //foreach (NewMod mod in mods)
                //{
   
                //    // might also want to check if the mod is the most up-to-date one (compared to possible other mods)
                //    if (!File.Exists(currentVersionInfo.saveFolder + mod.name + "_" + mod.version.CurrentVersion + extension) && !currentVersionInfo.installedVersion.IsNewestVersion(mod))
                //    {
                //        Console.WriteLine("Newer version found! Downloading file to " + currentVersionInfo.saveFolder + "...");
                //        //currentVersionInfo.updateChecker.GetFile(mod, currentVersionInfo.saveFolder, extension);
                //    }
                //    else
                //    {
                //        Console.WriteLine("You already have the newest version installed or downloaded!");
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void checker_FileDownloadCompleteEvent(string name)
        {
            try
            {
                this.Invoke(uText, "Download of file " + name + " successful!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateForm_Load(object sender, EventArgs e)
        {
            l_version.Text = currentVersion.installedVersion.version.CurrentVersion;
            GetModInfo();
        }

        private void b_download_Click(object sender, EventArgs e)
        {
            DownloadMod();
        }

        private void lb_availableUpdates_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DownloadMod();
        }

        private void lb_availableUpdates_SelectedIndexChanged(object sender, EventArgs e)
        {
            NewMod mod = (NewMod)lb_availableUpdates.SelectedItem;

            if (mod.alreadyPresent)
            {
                l_status.Text = "Already installed/downloaded";
            }
            else
            {
                l_status.Text = "Not installed/downloaded yet";
            }

            b_download.Enabled = !mod.alreadyPresent;
        }

        private void DownloadMod()
        {
            NewMod mod = (NewMod)lb_availableUpdates.SelectedItem;

            if (!mod.alreadyPresent)
            {
                // if the mod file defines its own extension use that one)
                string extension = mod.extension == null ? currentVersion.installedVersion.extension : mod.extension;

                currentVersion.updateChecker.GetFile(mod, currentVersion.saveFolder);
            }
        }
    }
}
