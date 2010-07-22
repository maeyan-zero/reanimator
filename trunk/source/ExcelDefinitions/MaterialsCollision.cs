using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MaterialsCollisionRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 materialNumber;
        public Int32 floor;//bool
        public Int32 mapsTo;//idx
        public float directOcclusion;
        public float reverbOcclusion;
        public Int32 debugColor;//idx
    }
}