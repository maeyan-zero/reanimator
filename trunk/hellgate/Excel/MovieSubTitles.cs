using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MovieSubTitles
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 movie0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 movie1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 movie2;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 movie3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LANGUAGE")]
        public Int32 language;//idx
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Strings")]
        public Int32 String;//stridx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined;
    }
}