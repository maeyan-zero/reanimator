using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds3D
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1, SortDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelAttribute(IsBool = true)]
        public Int32 front;
        [ExcelAttribute(IsBool = true)]
        public Int32 left;
        [ExcelAttribute(IsBool = true)]
        public Int32 right;
        [ExcelAttribute(IsBool = true)]
        public Int32 back;
        public Int32 undefined1;
        public Int32 undefined2;
        public Int32 undefined3;
        public Single minVolume;
        public Single maxVolume;
        public Single minIntersectDelay;
        public Single maxIntersectDelay;
        public Int32 minSetCount;
        public Int32 maxSetCount;
        public Single minIntrasetDelay;
        public Single maxIntrasetDelay;
        public Single setChance;
        public Int32 sound;//idx
    }
}