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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        Int32[] TCv4_1;
        public Int32 values1;
        public Int32 values2;
        public Int32 values3;
        public Int32 values4;
        public Int32 values5;
        public Int32 values6;
        public Int32 values7;
        public Int32 values8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] TCv4_2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        Int32[] TCv4_3;
        public float fadeInTimeInSeconds;
        public Int32 undefined1;
        public float fadeOutTimeInSeconds;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reverbOverRide;
        public Int32 undefined3;
        public Int32 undefined4;
    }
}