using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Procs : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ProcsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String proc;

            public Int32 id;
            public Int32 unknown1;
            public float unknown2;
            public Int32 unknown3;
            public float unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
            public Int32 unknown7;
            public Int32 unknown8;
        }

        public Procs(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ProcsTable>(data, ref offset, Count);
        }
    }
}
