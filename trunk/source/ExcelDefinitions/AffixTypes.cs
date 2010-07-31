using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixTypesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string AffixType;

        [ExcelFile.ExcelOutput(IsTableIndex = true, TableId = 0x3330 /*FONTCOLORS*/, Column = "Color")]
        public Int32 NameColor;
        public Int32 DownGrade;//idx
        public Int32 Required;//bool
    }
}
