using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class StateLighting : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StateLightingTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string sh_cubemap_filename;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] undefined1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 312)]
            byte[] unknown1;
    }

        public StateLighting(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StateLightingTable>(data, ref offset, Count);
        }
    }
}