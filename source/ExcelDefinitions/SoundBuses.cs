using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundBusesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 volume;
        public Int32 undefined;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        public Int32 sendsTo;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] vca;
        public Int32 userControl;
    }
}