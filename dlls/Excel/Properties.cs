using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Properties : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class PropertiesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] header;
            public Int32 property;
        }

        public Properties(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<PropertiesTable>(data, ref offset, Count);
        }
    }
}
