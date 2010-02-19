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
    class StringsTables
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

            String baseDataDir = Config.dataDirsRoot + @"\data\excel\strings\english\";
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

        public bool AddToDataSet(ProgressForm progress, Object arg)
        {
            DataSet ds = arg as DataSet;
            if (ds == null)
            {
                return false;
            }

            foreach (StringsFile stringsFile in stringsFiles)
            {
                progress.SetCurrentItemText(stringsFile.Name);

                if (ds.Tables.Contains(stringsFile.Name))
                {
                    continue;
                }

                DataTable dt = ds.Tables.Add(stringsFile.Name);
                foreach (StringsFile.StringBlock stringsBlock in stringsFile.StringsTable)
                {
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("ReferenceId", stringsBlock.ReferenceId.GetType());
                        dt.Columns.Add("Unknown1", stringsBlock.Unknown1.GetType());
                        dt.Columns.Add("StringId", stringsBlock.StringId.GetType());
                        dt.Columns.Add("Unknown2", stringsBlock.Unknown2.GetType());
                        dt.Columns.Add("String", stringsBlock.String.GetType());
                        dt.Columns.Add("Attribute1", stringsBlock.Attribute1.GetType());
                        dt.Columns.Add("Attribute2", stringsBlock.Attribute2.GetType());
                        dt.Columns.Add("Attribute3", stringsBlock.Attribute3.GetType());
                    }

                    dt.Rows.Add(stringsBlock.ReferenceId, stringsBlock.Unknown1, stringsBlock.StringId, stringsBlock.Unknown2,
                        stringsBlock.String, stringsBlock.Attribute1, stringsBlock.Attribute2, stringsBlock.Attribute3);
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
