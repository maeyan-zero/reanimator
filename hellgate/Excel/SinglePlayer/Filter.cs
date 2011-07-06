using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.SinglePlayer
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Filter
    {
        ExcelFile.RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String word;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 allowPrefixed;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 allowSuffixed;
    }
}
