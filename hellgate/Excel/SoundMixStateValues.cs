using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStateValues
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 undefined1;
        public Int32 busVolume;
        public float undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        public Int32 undefined3;
        public Int32 undefined4;
        public Int32 undefined5;
    }
}