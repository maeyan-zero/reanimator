using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class StatsFunc
    {
        RowHeader header;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 target;
        public App app;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 controlUnit;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4096)]
        public string formula;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        byte[] undefined2;

        public enum App
        {
            Null = -1,
            Common = 0,
            Hellgate = 1,
            Mythos = 2
        }
    }
}
