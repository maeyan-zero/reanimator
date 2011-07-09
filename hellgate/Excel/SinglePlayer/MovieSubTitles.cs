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
        public Language language;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 String;//stridx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined;

        public enum Language
        {
            Null = -1,
            English = 0,
            Korean = 1,
            ChineseSimplified = 2,
            ChineseTraditional = 3,
            Japanese = 4,
            French = 5,
            Spanish = 6,
            German = 7,
            Italian = 8,
            Polish = 9,
            Czech = 10,
            Hungarian = 11,
            Russian = 12,
            Thai = 13,
            Vietnamese = 14
        }
    }
}