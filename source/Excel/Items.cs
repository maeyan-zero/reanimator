using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Items : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ItemsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            Int32[] unknown;
            public Int16 id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 198)]
            public byte[] unknownData;
        }

        List<ItemsTable> items;

        public Items(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            items = ExcelTables.ReadTables<ItemsTable>(data, ref offset, Count);
        }
    }
}
