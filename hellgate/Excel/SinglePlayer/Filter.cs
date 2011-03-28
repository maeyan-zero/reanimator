using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Filter
    {
        RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 allowPrefixed;
        [ExcelOutput(IsBool = true)]
        public Int32 allowSuffixed;
    }
}
