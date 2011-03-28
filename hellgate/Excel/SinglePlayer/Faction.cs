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
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayString;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeStartStanding1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDefStartStanding1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION_STANDING")]
        public Int32 startStanding1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeStartStanding2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDefStartStanding2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION_STANDING")]
        public Int32 startStanding2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeStartStanding3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDefStartStanding3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION_STANDING")]
        public Int32 startStanding3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeStartStanding4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDefStartStanding4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION_STANDING")]
        public Int32 startStanding4;
    }
}