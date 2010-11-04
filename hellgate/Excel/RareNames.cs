using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RareNames
    {
        TableHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 suffix;//bool
        public Int32 level;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)] // changed to int.. problem reading last character
        public Int32 code;
        public Int32 types1;
        public Int32 types2;
        public Int32 types3;
        public Int32 types4;
        public Int32 types5;
        public Int32 types6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 cond;//intptr
    }
}
