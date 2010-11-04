using System;
using System.IO;
using System.Windows.Forms;

namespace Revival.Common
{
    public static class FormTools
    {
        public static String SaveFileDiag(String fileExtension, String typeName, String defaultFileName, String initialDirectory)
        {
            // This little function is here because for some reason AddExtension = false doesn't seem to do shit.
            // So basically I just check it manually.

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                AddExtension = false,
                DefaultExt = fileExtension,
                FileName = defaultFileName,
                Filter = String.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                saveFileDialog.Dispose();
                return null;
            }
            String filePath = saveFileDialog.FileName;
            saveFileDialog.Dispose();

            // since AddExtension = false doesn't seem to do shit
            string replaceExtension = "." + fileExtension;
            while (filePath.Contains(replaceExtension))
            {
                filePath = filePath.Replace(replaceExtension, "");
            }
            filePath += replaceExtension;

            if (!filePath.Contains(fileExtension))
            {
                filePath += fileExtension;
            }

            return filePath;
        }

        public static string OpenFileDiag(String fileExtension, String typeName, String initialDirectory)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = fileExtension,
                Filter = String.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                openFileDialog.Dispose();
                return null;
            }
            String filePath = openFileDialog.FileName;
            openFileDialog.Dispose();

            return filePath;
        }

        public static bool WriteFile(String filePath, byte[] byteData)
        {
            DialogResult dr = DialogResult.Yes;
            while (dr == DialogResult.Yes)
            {
                try
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteData, 0, byteData.Length);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    dr = MessageBox.Show("Failed to write to file!\nTry Again?\n\n" + e, "Error",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Error);
                }
            }

            return false;
        }
    }
}
