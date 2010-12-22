using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds2D
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
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
        public Int32 sound;//idx
    }
}