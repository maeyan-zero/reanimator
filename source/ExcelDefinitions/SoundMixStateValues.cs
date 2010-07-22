using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStateValuesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 undefined1;
        public Int32 busVolume;
        public float undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public Int32[] undefined3;
    }
}