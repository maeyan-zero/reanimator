using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Region
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionLong;
        [ExcelOutput(IsBool = true)]
        public Int32 isDefault;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionShort;
        public Int32 code;
    }
}
