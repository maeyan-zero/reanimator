using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class StringsFiles : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class StringsFilesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 isCommon;//bool
            public Int32 loadedbyGame;//bool
            public Int32 creditsFile;//bool
        }

        public StringsFiles(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StringsFilesTable>(data, ref offset, Count);
        }
    }
}
