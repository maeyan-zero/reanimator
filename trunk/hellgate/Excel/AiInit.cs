using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiInit
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string behaviourTable;
        public Int32 undefined1;
        public Int32 startFunction;//idx
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
        public Int32 giveStateToUnitType;//idx
        public float giveStateRange;
        public Int32 stateToGive;//idx
    }
}
