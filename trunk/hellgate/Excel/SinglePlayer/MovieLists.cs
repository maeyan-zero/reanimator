using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MovieLists
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1f;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1g;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list1h;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2a;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2b;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2c;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2d;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2e;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2f;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2g;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIES")]
        public Int32 list2h;
    }
}
