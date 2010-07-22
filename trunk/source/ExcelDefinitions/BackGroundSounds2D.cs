using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds2DRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public float undefined1;
        public float undefined2;
        public float minVolume;
        public float maxVolume;
        public float silentChance;
        public float minPlayTime;
        public float maxPlayTime;
        public float minSilentTime;
        public float maxSilentTime;
        public float fadeIn;
        public float fadeOut;
        public Int32 sound;//idx
    }
}