using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class InvLoc : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InvLocTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string name;
            public short code;
        }

        public InvLoc(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InvLocTable>(data, ref offset, Count);
        }
    }
}