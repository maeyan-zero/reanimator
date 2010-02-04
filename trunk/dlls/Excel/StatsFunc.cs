using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class StatsFunc : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatsFuncTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            public Int32 target;
            public Int32 app;
            public Int32 controlUnit;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
            public string formula;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            byte undefined2;
   }

        public StatsFunc(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StatsFuncTable>(data, ref offset, Count);
        }
    }
}