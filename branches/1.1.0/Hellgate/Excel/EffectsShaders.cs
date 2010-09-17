using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsShaders
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String name;
        public Int32 indoorEffect;//idx
        public Int32 outdoorEffect;//idx
        public Int32 indoorGridEffect;//idx
        public Int32 flashLightEffect;//idx
        [ExcelAttribute(IsBool = true)]
        public Int32 noCollide;
        [ExcelAttribute(IsBool = true)]
        public Int32 forParticles;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] undefined;
    }
}