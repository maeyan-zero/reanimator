using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BookMarks : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BookMarksTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public short code;
        }

        public BookMarks(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BookMarksTable>(data, ref offset, Count);
        }
    }
}