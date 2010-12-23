using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsShaders
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
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
        public Int32 undefined1a;
        public Int32 undefined1b;
        public Int32 undefined1c;
        public Int32 undefined1d;
        public Int32 undefined1e;
        public Int32 undefined1f;
        public Int32 undefined1g;
        public Int32 undefined1h;
    }
}