using System;
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
            public string AffixType;

            [ExcelOutput(IsTableIndex = true, TableId = 0x3330 /*FONTCOLORS*/, Column = "Color")]
            public Int32 NameColor;

            public Int32 DownGrade;//idx
            public Int32 Required;//bool
        }

        public AffixTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AffixTypesTable>(data, ref offset, Count);
        }
    }
}
