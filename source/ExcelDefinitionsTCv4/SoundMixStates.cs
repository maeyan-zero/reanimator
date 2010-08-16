using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStatesTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
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
        public Int32 undefined1;
        public float fadeOutTimeInSeconds;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reverbOverRide;
        public Int32 undefined3;
        public Int32 undefined4;
        public Int32 undefined5;
    }
}