using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class States : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            public Int32 stringIdOffset;
            public Int32 unknown;
            public Int16 id;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 198)]
            byte[] unknownData;
        }

        List<StatesTable> states;

        public States(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return states.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            states = ExcelTables.ReadTables<StatesTable>(data, ref offset, Count);
        }
    }
}
