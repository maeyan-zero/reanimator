using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MiniGameTagBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MINIGAME_TYPE")]//table C3
        public Int32 miniGameType;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "DAMAGETYPES")]//table 1E
        public Int32 goalDamageType;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 goalUnitType;
        public Int32 minNeeded;
        public Int32 maxNeeded;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string frameName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string achievedFrameName;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 toolTip;
    }
}
