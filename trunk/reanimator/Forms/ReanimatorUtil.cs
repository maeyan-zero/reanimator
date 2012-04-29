using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Hellgate;
using Reanimator.Properties;
using Revival.Common;

namespace Reanimator.Forms
{
    partial class Reanimator
    {
        /// <summary>
        /// Checks the Windows Version and sets the coresponding icon
        /// </summary>
        private void _SetIcon()
        {
            OperatingSystem osInfo = Environment.OSVersion;
            Icon pathName = Resources.icon2;

            if (osInfo.Platform == PlatformID.Win32NT)
            {
                switch (osInfo.Version.Major)
                {
                    case 5: // 5 = XP/2000/2003 server edition
                        pathName = Resources.icon1;
                        break;

                    case 6: // 6 = Vista
                        pathName = Resources.icon2;
                        break;

                    default:
                        pathName = Resources.icon2;
                        break;
                }
            }

            Icon = pathName;
        }

        /// <summary>
        /// Checks the registry for the Hellgate path, if it doesn't exist prompt the user to find it.
        /// </summary>
        /// <returns>True if the installation is okay.</returns>
        private static bool _CheckInstallation()
        {
            if (Directory.Exists(Config.HglDir)) return true;

            string caption = "Reanimator Installation";
            string message = "Please locate your Hellgate London installation directory.\n" +
                             "For this program to work correctly, please ensure the latest Single Player patch is installed.\n" +
                             "For more information, please visit our website: http://www.hellgateaus.net";
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            DialogResult installResult;

            do
            {
                DialogResult selectPathResult = folderBrowser.ShowDialog();
                if ((selectPathResult == DialogResult.OK))
                {
                    Config.HglDir = folderBrowser.SelectedPath;
                    Config.HglDataDir = Path.Combine(Config.HglDir, "\\data");
                    return true;
                }

                caption = "Installation Error";
                message = "You must have Hellgate: London installed and the directory set to use Reanimator.";
                installResult = MessageBox.Show(message, caption, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
            while (installResult == DialogResult.Retry);

            return false;
        }

        private void _DoLoadingThread(ProgressForm progressForm, Object var)
        {
            //// Single Player/Standard data files ////
            progressForm.SetCurrentItemText("Loading File Manager...");
            _fileManager = new FileManager(Config.HglDir);
            //_fileManager = new FileManager(Config.HglDir, FileManager.ClientVersions.Mod);

            foreach (PackFile file in _fileManager.IndexFiles) file.BeginDatReading();
            progressForm.SetCurrentItemText("Loading Excel and Strings Tables...");
            if (!_fileManager.LoadTableFiles())
            {
                MessageBox.Show("Failed to load excel and strings files!", "Data Table Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (PackFile file in _fileManager.IndexFiles) file.EndDatAccess();

            if (_fileManager.IsVersionMod) _fileManager.ProcessTables();

            if (!Config.LoadTCv4DataFiles) return;


            //// TCv4 data files ////
            progressForm.SetCurrentItemText("Loading TCv4 File Manager...");
            _fileManagerTCv4 = new FileManager(Config.HglDir, FileManager.ClientVersions.TestCenter);

            foreach (PackFile file in _fileManagerTCv4.IndexFiles) file.BeginDatReading();
            progressForm.SetCurrentItemText("Loading TCv4 Excel and Strings Tables...");
            if (!_fileManagerTCv4.LoadTableFiles())
            {
                MessageBox.Show("Failed to load TCv4 excel and strings files!", "Data Table Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            foreach (PackFile file in _fileManagerTCv4.IndexFiles) file.EndDatAccess();
        }

        private void _OpenFileExplorer()
        {
            if (_fileExplorer != null && !_fileExplorer.IsDisposed) // it works - but probably should be tested a bit more if we keep this option
            {
                DialogResult dr = MessageBox.Show("An instance of FileExplorer is already open.\nDo you want to open another instance? (Not fully tested)", "Already Open",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No) return;
            }

            ProgressForm progressForm = new ProgressForm(_LoadFileExplorerThread, null);
            progressForm.SetStyle(ProgressBarStyle.Marquee);
            progressForm.SetLoadingText("Initializing File Explorer...");
            progressForm.SetCurrentItemText("");
            progressForm.Disposed += delegate { _fileExplorer.Show(); };
            progressForm.Show(this);
        }

        private void _LoadFileExplorerThread(ProgressForm progress, object obj)
        {
            _fileExplorer = new FileExplorer(_fileManager, _fileManagerTCv4);
        }

        private void _OpenExcelTableEditor()
        {
            if (_excelTableForm == null || _excelTableForm.IsDisposed)
            {
                _excelTableForm = new TableEditorForm(_fileManager)
                {
                    MdiParent = this,
                    Text = "Table Editor [" + _fileManager.ClientVersion + "]"
                };
            }

            _excelTableForm.Show();
            _excelTableForm.Focus();
        }

        private void _OpenExcelTableEditorTCv4()
        {
            if (!Config.LoadTCv4DataFiles) return;

            if (_excelTableFormTCv4 == null || _excelTableFormTCv4.IsDisposed)
            {
                _excelTableFormTCv4 = new TableEditorForm(_fileManagerTCv4)
                {
                    MdiParent = this,
                    Text = "Table Editor [" + _fileManagerTCv4.ClientVersion + "]"
                };
            }

            _excelTableFormTCv4.Show();
            _excelTableFormTCv4.Focus();
        }
    }
}
