using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class FootSteps : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FootStepsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 concrete;//idx
            public Int32 wood;//idx
            public Int32 metal;//idx
            public Int32 tile;//idx
            public Int32 squishy;//idx
            public Int32 gravel;//idx
            public Int32 snow;//idx
            public Int32 dirt;//idx
            public Int32 water;//idx
            public Int32 rubble;//idx
            public Int32 undefined;//idx
        }

        public FootSteps(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FootStepsTable>(data, ref offset, Count);
        }
    }
}
