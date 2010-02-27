using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Palettes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class PalettesTable
        {
            TableHeader header;

            public Int32 unknown1;
            public Int32 unknown2;
            public Int32 unknown3;
            public Int32 unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
        }

        public Palettes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<PalettesTable>(data, ref offset, Count);
        }
    }
}
