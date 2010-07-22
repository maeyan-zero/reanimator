using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds3DRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 front;//bool
        public Int32 left;//bool
        public Int32 right;//bool
        public Int32 back;//bool
        public Int32 undefined1;
        public Int32 undefined2;
        public Int32 undefined3;
        public float minVolume;
        public float maxVolume;
        public float minIntersectDelay;
        public float maxIntersectDelay;
        public Int32 minSetCount;
        public Int32 maxSetCount;
        public float minIntrasetDelay;
        public float maxIntrasetDelay;
        public float setChance;
        public Int32 sound;//idx
    }
}