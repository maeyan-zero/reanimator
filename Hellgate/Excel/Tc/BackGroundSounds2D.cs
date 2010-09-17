using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds2DTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 undefined1;
        public Int32 undefined2;
        public float minVolume;
        public float maxVolume;
        public float silentChance;
        public float minPlayTime;
        public float maxPlayTime;
        public float minSilentTime;
        public float maxSilentTime;
        public float fadeIn;
        public float fadeOut;
        public Int32 undefined_TCV4_1;
        public Int32 ADSR_tcv4;
        public Int32 undefined_TCV4_3;
        public Int32 undefined_TCV4_4;
        public Int32 undefined_TCV4_5;
        public Int32 sound;//idx
        public Int32 undefined_TCV4_6;
    }
}