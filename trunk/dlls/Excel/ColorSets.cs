using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ColorSets : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ColorSetsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string color;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            Int32[] undefined;
            public Int32 code;
            public Int32 canBeRandomPick;//bool
        }

        public ColorSets(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ColorSetsTable>(data, ref offset, Count);
        }
    }
}
