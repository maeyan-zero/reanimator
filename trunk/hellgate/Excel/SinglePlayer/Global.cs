using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Global
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String String;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String string_tugboat;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 dataTable;//tbl
        Int32 unknown2;             // always 0
        Int32 unknown3;             // always 0
        Int32 unknown4;             // always 0
    }
}
