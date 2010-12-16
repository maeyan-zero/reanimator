using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds3D
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 front;
        [ExcelOutput(IsBool = true)]
        public Int32 left;
        [ExcelOutput(IsBool = true)]
        public Int32 right;
        [ExcelOutput(IsBool = true)]
        public Int32 back;
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