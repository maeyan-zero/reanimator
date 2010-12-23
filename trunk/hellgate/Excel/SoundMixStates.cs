using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStates
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 priority;
        public Int32 values1;
        public Int32 values2;
        public Int32 values3;
        public Int32 values4;
        public Int32 values5;
        public Int32 values6;
        public Int32 values7;
        public Int32 values8;
        public float fadeInTimeInSeconds;
        Int32 undefined1; // is always 0
        public float fadeOutTimeInSeconds;
        Int32 undefined2; // is always 0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reverbOverRide;
        Int32 undefined3; // is always 0
        Int32 undefined4; // is always 0
        Int32 undefined5; // is always 0
    }
}