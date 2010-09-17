using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds2D
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1, SortDefault = true)]
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