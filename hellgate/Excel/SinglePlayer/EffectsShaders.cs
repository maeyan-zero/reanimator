using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EffectsShaders
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS")]
        public Int32 indoorEffect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS")]
        public Int32 outdoorEffect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS")]
        public Int32 indoorGridEffect;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS")]
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