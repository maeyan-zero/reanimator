using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InitDb
    {
        RowHeader header;
        public Int32 skip;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string criteria;
        public Int32 rangeLow;
        public Int32 rangeHigh;
        public Int32 numMin;
        public Int32 numMax;
        public Int32 numInit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string featKnob;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string featMin;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string featMax;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string featInit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string numKnob;
    }
}
