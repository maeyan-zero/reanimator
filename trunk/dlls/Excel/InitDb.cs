using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class InitDb : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InitDbTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            public Int32 skip;//bool
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string criteria;

            public Int32 rangeLow;
            public Int32 rangeHigh;
            public float numMin;
            public float numMax;
            public float numInit;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string featKnob;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string featMin;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string featMax;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string featInit;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string numKnob;

        }

        public InitDb(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InitDbTable>(data, ref offset, Count);
        }
    }
}
