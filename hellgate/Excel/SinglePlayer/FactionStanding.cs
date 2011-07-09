using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionStanding
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayStringNumbers;
        public Int32 minScore;
        public Int32 maxScore;
        public Mood mood;

        public enum Mood
        {
            Bad = 0,
            Neutral = 1,
            Good = 2
        }
    }
}
