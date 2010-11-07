using System;
using System.Collections.Generic;
using System.Data;

namespace Hellgate
{
    public abstract class DataFile
    {
        public String StringID { get; protected set; }
        public Type DataType { get; protected set; }
        public uint StructureID { get; protected set; }
        public bool IntegrityCheck { get; protected set; }
        public bool IsExcelFile { get; protected set; }
        public bool IsStringsFile { get; protected set; }
        public int Count { get { return (!(Rows == null)) ? Rows.Count : 0; } }
        public String FilePath { get; set; }
        public String FileExtension { get; set; }
        public String FileName { get; set; }
        public List<Object> Rows { get; protected set; }

        public override String ToString()
        {
            return StringID;
        }
        public abstract bool ParseData(byte[] buffer);
        public abstract bool ParseCSV(byte[] buffer);
        public abstract bool ParseDataTable(DataTable dataTable);
        public abstract byte[] ToByteArray();
    }
}
