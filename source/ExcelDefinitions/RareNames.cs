using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RareNamesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 suffix;//bool
        public Int32 level;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Int32[] types;
        public Int32 cond;//intptr
    }
}