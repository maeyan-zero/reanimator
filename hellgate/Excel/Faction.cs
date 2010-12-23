using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Faction
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Strings")]
        public Int32 displayString;
        public Int32 unitTypeStartStanding1;
        public Int32 levelDefStartStanding1;
        public Int32 startStanding1;
        public Int32 unitTypeStartStanding2;
        public Int32 levelDefStartStanding2;
        public Int32 startStanding2;
        public Int32 unitTypeStartStanding3;
        public Int32 levelDefStartStanding3;
        public Int32 startStanding3;
        public Int32 unitTypeStartStanding4;
        public Int32 levelDefStartStanding4;
        public Int32 startStanding4;
    }
}