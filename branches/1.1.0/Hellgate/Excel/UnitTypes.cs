using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitTypes
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1, SortDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String type;
        [ExcelAttribute(SortID = 2)]
        public UInt32 code;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA0;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA6;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA7;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA8;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA9;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA10;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA11;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA12;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA13;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA14;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 isA15;
        public Int32 String;
    }
}