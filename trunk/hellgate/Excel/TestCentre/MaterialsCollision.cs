using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MaterialsCollisionTCv4
    {
        ExcelFile.RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 materialNumber;
        [ExcelOutput(IsBool = true)]
        public Int32 floor;//bool
        public Int32 noCollide_tcv4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MATERIALS_COLLISION")]
        public Int32 mapsTo;//idx
        public float directOcclusion;
        public float reverbOcclusion;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 debugColor;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string max9Name_tcv4;
    }
}