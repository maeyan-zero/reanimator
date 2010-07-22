using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStatesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 priority;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] values;
        public float fadeInTimeInSeconds;
        public Int32 undefined1;
        public float fadeOutTimeInSeconds;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reverbOverRide;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] undefined3;
    }
}