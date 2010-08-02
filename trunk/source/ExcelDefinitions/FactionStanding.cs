using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionStandingRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 displayString;
        [ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 displayStringNumbers;
        public Int32 minScore;
        public Int32 maxScore;
        public Int32 mood;
    }
}