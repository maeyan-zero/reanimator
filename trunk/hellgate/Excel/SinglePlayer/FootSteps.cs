using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FootSteps
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 concrete;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 wood;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 metal;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 tile;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 squishy;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 gravel;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 snow;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 dirt;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 water;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 rubble;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 undefined;//idx
    }
}