using System;
using System.Collections.Generic;
using System.IO;
using Reanimator.Forms;

namespace Reanimator
{
    public class StringsTables
    {
        readonly List<StringsFile> _stringsFiles;

        public StringsTables()
        {
            _stringsFiles = new List<StringsFile>();
        }

        public bool LoadStringsTables(ProgressForm progress, Object arg)
        {
            StringsFiles stringsFiles = arg as StringsFiles;
            if (stringsFiles == null)
            {
                return false;
            }

            String baseDataDir = Config.DataDirsRoot + @"\data\excel\strings\english\";
            const string fileExtension = ".xls.uni.cooked";

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
                        StringsFile stringsFile = new StringsFile(stringTable.name, null)
                                                      {
                                                          FilePath = path
                                                      };
                        stringsFile.ParseData(FileTools.StreamToByteArray(fs));
                        if (stringsFile.IsGood)
                        {
                            _stringsFiles.Add(stringsFile);
                        }
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
            return _stringsFiles;
        }

        public int Count
        {
            get { return _stringsFiles.Count; }
        }
    }
}
