using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class States : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct StatesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            Int32[] unknown;

            private Int16 id;
            public Int16 Id
            {
                get { return id; }
                set { id = value; }
            }

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 198)]
            public byte[] unknownData;
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
