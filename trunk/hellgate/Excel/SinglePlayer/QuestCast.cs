using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestCast
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType0;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster0;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType4;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster4;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType5;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster5;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType6;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster6;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitType7;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monster7;//idx
    }
}
