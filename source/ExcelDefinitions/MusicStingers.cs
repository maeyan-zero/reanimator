using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicStingersRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 type;
        public Int32 fadeOutBeats;
        public Int32 fadeInBeats;
        public Int32 fadeInDelayBeats;
        public Int32 fadeOutDelayBeats;
        public Int32 introBeats;
        public Int32 outroBeats;
        public Int32 soundGroup;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined;
    }
}