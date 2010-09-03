using System;
using System.Collections.Generic;
using System.Data;

namespace Reanimator
{
    public abstract class DataFile
    {
        public String StringId { get; private set; }
        public Type DataType { get; private set; }

        public bool IsExcelFile { get; protected set; }
        public bool IsStringsFile { get; protected set; }
        public bool IsGood { get; protected set; }
        public int Count { get; protected set; }

        public String FilePath { get; set; }
        public String FileExtension { get; set; }
        public String FileName { get; set; }

        internal byte[] _data;
        public List<object> Rows { get; set; }

        protected DataFile(String stringId, Type dataType)
        {
            if (String.IsNullOrEmpty(stringId)) throw new Exception("if (String.IsNullOrEmpty(stringId))");
            if (dataType == null) throw new Exception("if (dataType == null)");

            StringId = stringId;
            DataType = dataType;

            IsExcelFile = false;
            IsStringsFile = false;
            IsGood = false;
            Count = -1;

            FilePath = String.Empty;

            _data = null;
            Rows = new List<object>();
        }

        override public String ToString()
        {
            return StringId;
        }

        public abstract bool ParseData(byte[] data);
        public abstract byte[] GenerateFile(DataTable dataTable);
    }
}
