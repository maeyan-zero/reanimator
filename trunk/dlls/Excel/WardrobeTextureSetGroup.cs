using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobeTextureSetGroup : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeTextureSetGroupTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 appearanceGroupCategory;
        }

        public WardrobeTextureSetGroup(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeTextureSetGroupTable>(data, ref offset, Count);
        }
    }
}
