using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiInit
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public String behaviourTable;
        public Int32 undefined1;
        public Int32 startFunction;//idx
        public Int32 blockingState;//idx
        public Int32 undefined2;
        [ExcelAttribute(IsBool = true)]
        public Int32 wantsTarget;
        [ExcelAttribute(IsBool = true)]
        public Int32 targetClosest;
        [ExcelAttribute(IsBool = true)]
        public Int32 randomizetargets;
        [ExcelAttribute(IsBool = true)]
        public Int32 noDestructables;
        [ExcelAttribute(IsBool = true)]
        public Int32 randomStartPeriod;
        [ExcelAttribute(IsBool = true)]
        public Int32 dontFreeze;
        [ExcelAttribute(IsBool = true)]
        public Int32 recordSpawnPoint;
        [ExcelAttribute(IsBool = true)]
        public Int32 checkBusy;
        [ExcelAttribute(IsBool = true)]
        public Int32 startOnAiInit;
        [ExcelAttribute(IsBool = true)]
        public Int32 neverRegisterAi;
        [ExcelAttribute(IsBool = true)]
        public Int32 clientOnly;
        public Int32 udefined3;
        [ExcelAttribute(IsBool = true)]
        public Int32 canSeeUnsearchables;
        [ExcelAttribute(IsBool = true)]
        public Int32 giveStateOnDeath;
        public Int32 giveStateToUnitType;//idx
        public Single giveStateRange;
        public Int32 stateToGive;//idx
    }
}