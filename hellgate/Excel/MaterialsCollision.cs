using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MaterialsCollision
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 materialNumber;
        [ExcelOutput(IsBool = true)]
        public Int32 floor;//bool
        public Int32 mapsTo;//idx
        public float directOcclusion;
        public float reverbOcclusion;
        public Int32 debugColor;//idx
    }
}
