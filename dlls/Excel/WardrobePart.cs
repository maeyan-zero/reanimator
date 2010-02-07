using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobePart : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobePartTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
            public string label;
            public Int32 partGroup;
            public Int32 materialIndex;
            public Int32 targetTexture;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string materialName;
        }

        public WardrobePart(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobePartTable>(data, ref offset, Count);
        }
    }
}
