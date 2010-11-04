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
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Strings")]
        public Int32 displayString;
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Strings")]
        public Int32 displayStringNumbers;
        public Int32 minScore;
        public Int32 maxScore;
        public Int32 mood;
    }
}
