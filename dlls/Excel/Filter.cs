using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Filter : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FilterTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string name;

            public Int32 unknown1;
            public Int32 unknown2;
        }

        public Filter(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FilterTable>(data, ref offset, Count);
        }
    }
}
