using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Act
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelAttribute(SortID = 2)]
        public Int32 code;
        [ExcelAttribute(IsBitmask = true)]
        public BitMask01 bitmask01;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            betaAccountCanPlay = 1,
            nonSubScriberAccountCanPlay = 2
        }
    }
}
