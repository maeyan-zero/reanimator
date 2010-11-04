using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsShaders
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        public Int32 indoorEffect;//idx
        public Int32 outdoorEffect;//idx
        public Int32 indoorGridEffect;//idx
        public Int32 flashLightEffect;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 noCollide;
        [ExcelOutput(IsBool = true)]
        public Int32 forParticles;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] undefined;
    }
}
