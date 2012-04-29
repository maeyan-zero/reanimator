using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Act
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            betaAccountCanPlay = 1,
            nonSubScriberAccountCanPlay = 2
        }
    }
}
