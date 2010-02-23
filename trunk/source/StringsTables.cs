using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Reanimator.Excel;
using System.IO;
using Reanimator.Forms;
using System.Data;

namespace Reanimator
{
    public class StringsTables
    {
        List<StringsFile> stringsFiles;

        public StringsTables()
        {
            stringsFiles = new List<StringsFile>();
        }

        public bool LoadStringsTables(ProgressForm progress, Object arg)
        {
            StringsFiles stringsFiles = arg as StringsFiles;
            if (stringsFiles == null)
            {
                return false;
            }

            String baseDataDir = Config.DataDirsRoot + @"\data\excel\strings\english\";
            String fileExtension = ".xls.uni.cooked";

            foreach (StringsFiles.StringsFilesTable stringTable in (Object[])stringsFiles.GetTableArray())
            {
                String path = baseDataDir + stringTable.name + fileExtension;
                if (!File.Exists(path))
                {
                    path = path.Replace("data", "data_common");
                    if (!File.Exists(path))
                    {
                        continue;
                    }
                }

                progress.SetCurrentItemText(stringTable.name);

                try
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        StringsFile stringsFile = new StringsFile(FileTools.StreamToByteArray(fs));
                        stringsFile.Name = stringTable.name;
                        this.stringsFiles.Add(stringsFile);
                        fs.Close();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return true;
        }

        public List<StringsFile> GetLoadedTables()
        {
            return stringsFiles;
        }

        public int Count
        {
            get { return stringsFiles.Count; }
        }
    }
}
