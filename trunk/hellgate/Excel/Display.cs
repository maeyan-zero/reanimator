using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Display
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string key;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string exampleDescription;
        [ExcelOutput(IsBool = true)]
        public Int32 rider;
        public Int32 rule1;
        public Int32 rule2;
        public Int32 rule3;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond1;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond2;
        [ExcelOutput(IsBool = true)]
        public Int32 inclUnitInCond3;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition1;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition2;
        [ExcelOutput(IsScript = true)]
        public Int32 displayCondition3;
        [ExcelOutput(IsBool = true)]
        public Int32 newLine;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 formatString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 formatShort;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 decripShort;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        string iconFrame;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 ctrlStat;//idx
        public Int32 displayCtrl;
        public Int32 displayFunc;
        public Int32 ctrl1;
        public Int32 ctrl2;
        public Int32 ctrl3;
        public Int32 ctrl4;
        [ExcelOutput(IsScript = true)]
        public Int32 val1;
        [ExcelOutput(IsScript = true)]
        public Int32 val2;
        [ExcelOutput(IsScript = true)]
        public Int32 val3;
        [ExcelOutput(IsScript = true)]
        public Int32 val4;
        public Int32 toolTipArea;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 toolTipText;//stridx
        [ExcelOutput(IsScript = true)]
        public Int32 color;
    }
}