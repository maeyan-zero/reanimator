using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Global
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String String;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String string_tugboat;
        public Int32 dataTable;//tbl
        Int32 unknown2;             // always 0
        Int32 unknown3;             // always 0
        Int32 unknown4;             // always 0
    }
}
