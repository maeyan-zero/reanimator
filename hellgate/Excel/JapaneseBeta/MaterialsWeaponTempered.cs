using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MaterialsWeaponTemperedBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string globalMaterialName;
        public float brightnessToMul;
        public Int32 blinkMillis;
        public float shineSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string particle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string boneToAttach;
        public float shineSizeForMelee;
    }
}
