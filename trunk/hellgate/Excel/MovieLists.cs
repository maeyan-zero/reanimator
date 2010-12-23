using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MovieLists
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 list1a;
        public Int32 list1b;
        public Int32 list1c;
        public Int32 list1d;
        public Int32 list1e;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list1f;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list1g;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list1h;
        public Int32 list2a;
        public Int32 list2b;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2c;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2d;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2e;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2f;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2g;
        [ExcelOutput(ConstantValue = -1)]
        Int32 list2h;
    }
}
