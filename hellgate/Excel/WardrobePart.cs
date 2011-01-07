using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobePart
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string label;
        public Int32 partGroup; // XLS_InternalIndex_PartGroup (XLS_WARDROBE_MODEL_PART_DATA+101), 0x05
        public Int32 materialIndex;
        public Int32 targetTexture; // XLS_InternalIndex_TargetTexture (XLS_WARDROBE_MODEL_PART_DATA+CE), 0x09
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string materialName;
    }
}