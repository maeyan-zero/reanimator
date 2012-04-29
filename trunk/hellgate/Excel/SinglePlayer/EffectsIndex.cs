using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsIndex
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 fixedFunc;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 sm_11;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 sm_20_Low;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 sm_20_High;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 sm_30;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "EFFECTS_FILES")]
        public Int32 sm_40;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 required;
    }
}
