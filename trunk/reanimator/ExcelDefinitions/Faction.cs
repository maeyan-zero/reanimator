using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Strings")]
        public Int32 displayString;
        public Int32 unitTypeStartStanding1;
        Int32 levelDefStartStanding1;
        public Int32 startStanding1;
        public Int32 unitTypeStartStanding2;
        public Int32 levelDefStartStanding2;
        public Int32 startStanding2;
        Int32 unitTypeStartStanding3;
        Int32 levelDefStartStanding3;
        Int32 startStanding3;
        Int32 unitTypeStartStanding4;
        Int32 levelDefStartStanding4;
        Int32 startStanding4;
    }
}