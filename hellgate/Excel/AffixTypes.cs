using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixTypes
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string AffixType;
        [ExcelOutput(IsTableIndex = true, TableStringID = "FONTCOLORS")]
        public Int32 NameColor;
        public Int32 DownGrade;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 Required;
    }
}
