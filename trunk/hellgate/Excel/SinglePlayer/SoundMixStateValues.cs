using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStateValues
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 undefined1;
        public Int32 busVolume;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public float undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        public Int32 undefined3; // always 0 in SP, not TCv4 though
        Int32 undefined4; // always 0
        Int32 undefined5; // always 0
    }
}