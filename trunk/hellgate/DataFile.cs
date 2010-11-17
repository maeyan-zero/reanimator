using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Hellgate
{
    public abstract class DataFile
    {
        public string StringID { get; protected set; }
        public Type DataType { get; protected set; }
        public uint StructureID { get; protected set; }
        public bool IntegrityCheck { get; protected set; }
        public bool IsExcelFile { get; protected set; }
        public bool IsStringsFile { get; protected set; }
        public int Count { get { return (!(Rows == null)) ? Rows.Count : 0; } }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get { return Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(FilePath)); } }
        public List<object> Rows { get; protected set; }

        public override string ToString()
        {
            return StringID;
        }
        public abstract bool ParseData(byte[] buffer);
        public abstract bool ParseCSV(byte[] buffer);
        public abstract bool ParseDataTable(DataTable dataTable);
        public abstract byte[] ToByteArray();
        public abstract byte[] ExportCSV();
    }
}
