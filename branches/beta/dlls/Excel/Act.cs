using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Act : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ActTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 bitmask;/*0 bit beta account can play
	1 bit non_subscriber account can play*/
        }

        public Act(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ActTable>(data, ref offset, Count);
        }
    }
}
