using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ItemLookGroups : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ItemLookGroupsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public byte undefined;
        }

        public ItemLookGroups(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ItemLookGroupsTable>(data, ref offset, Count);
        }
    }
}
