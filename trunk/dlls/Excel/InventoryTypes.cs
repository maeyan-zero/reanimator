using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class InventoryTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InventoryTypesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
    }

        public InventoryTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InventoryTypesTable>(data, ref offset, Count);
        }
    }
}