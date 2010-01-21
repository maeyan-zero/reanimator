using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    class States : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct StatesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            Int32[] unknown;
            public Int16 id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 198)]
            public byte[] unknownData;
        }

        List<StatesTable> states;

        public States(byte[] data) : base(data) {}

        protected override void ParseTables(byte[] data)
        {
            states = ExcelTables.ReadTables<StatesTable>(data, ref offset, Count);
        }
    }
}
