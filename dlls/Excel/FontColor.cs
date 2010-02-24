using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class FontColor : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class FontColorTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Color;
            public byte blue;
            public byte green;
            public byte red;
            public byte alpha;
            public Int32 fontColor;//index

        }

        public FontColor(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<FontColorTable>(data, ref offset, Count);
        }
    }
}
