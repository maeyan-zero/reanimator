using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ItemLooks : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ItemLooksTable
        {
            TableHeader header;

            public Int32 item;//idx
            public Int32 lookGroup;//idx
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string folder;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string name;
            public Int32 wardrobe;//idx;
            public Int32 undefined2;
        }

        public ItemLooks(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ItemLooksTable>(data, ref offset, Count);
        }
    }
}