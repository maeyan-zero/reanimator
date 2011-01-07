using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Weather
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme4;
    }
}