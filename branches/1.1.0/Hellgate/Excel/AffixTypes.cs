using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixTypes
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string affixType;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "FONTCOLORS")]
        public Int32 nameColor;
        public Int32 downGrade;//idx
        [ExcelAttribute(IsBool = true)]
        public Int32 required;
    }
}
