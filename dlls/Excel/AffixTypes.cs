using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class AffixTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AffixTypesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string affixType;

            public Int32 nameColor;//idx
            public Int32 downGrade;//idx
            public Int32 required;//bool
        }

        public AffixTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AffixTypesTable>(data, ref offset, Count);
        }
    }
}
