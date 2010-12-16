using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionStanding
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Strings")]
        public Int32 displayString;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Strings")]
        public Int32 displayStringNumbers;
        public Int32 minScore;
        public Int32 maxScore;
        public Int32 mood;
    }
}
