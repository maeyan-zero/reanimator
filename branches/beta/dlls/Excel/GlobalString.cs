using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class GlobalString : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GlobalStringTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stringId;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string unknownString1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string unknownString2;

            // always [-1, 0, 0, 0] - assuming reserved or something
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] reserved;
        }

        public GlobalString(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GlobalStringTable>(data, ref offset, Count);
        }
    }
}
