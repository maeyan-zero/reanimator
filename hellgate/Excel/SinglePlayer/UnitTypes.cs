using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.SinglePlayer
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class UnitTypes
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String type;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA7;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA8;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA9;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA10;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA11;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA12;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA13;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA14;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 isA15;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 String;//stridx
    }
}