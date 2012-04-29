using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.SinglePlayer
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ColorSets
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string color;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 canBeRandomPick;
    }
}
