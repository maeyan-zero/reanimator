using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Npc
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingGeneric;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingTemplar;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingCabalist;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingHunter;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingMale;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingFemale;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingFactionBad;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingFactionNeutral;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 greetingFactionGood;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeGeneric;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeTemplar;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeCabalist;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeHunter;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeMale;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeFemale;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeFactionBad;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeFactionNeutral;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 goodByeFactionGood;//idx
    }
}
