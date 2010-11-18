using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class GlobalRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
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