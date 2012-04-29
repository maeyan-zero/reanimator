using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiInit
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string behaviourTable;
        public Int32 undefined1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AI_START")]
        public Int32 startFunction;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 blockingState;//idx
        public Int32 undefined2;
        [ExcelOutput(IsBool = true)]
        public Int32 wantsTarget;
        [ExcelOutput(IsBool = true)]
        public Int32 targetClosest;
        [ExcelOutput(IsBool = true)]
        public Int32 randomizetargets;
        [ExcelOutput(IsBool = true)]
        public Int32 noDestructables;
        [ExcelOutput(IsBool = true)]
        public Int32 randomStartPeriod;
        [ExcelOutput(IsBool = true)]
        public Int32 dontFreeze;
        [ExcelOutput(IsBool = true)]
        public Int32 recordSpawnPoint;
        [ExcelOutput(IsBool = true)]
        public Int32 checkBusy;
        [ExcelOutput(IsBool = true)]
        public Int32 startOnAiInit;
        [ExcelOutput(IsBool = true)]
        public Int32 neverRegisterAi;
        [ExcelOutput(IsBool = true)]
        public Int32 clientOnly;
        public Int32 udefined3;
        [ExcelOutput(IsBool = true)]
        public Int32 canSeeUnsearchables;
        [ExcelOutput(IsBool = true)]
        public Int32 giveStateOnDeath;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 giveStateToUnitType;//idx
        public float giveStateRange;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 stateToGive;//idx
    }
}
