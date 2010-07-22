using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AiInitRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string behaviourTable;
        public Int32 undefined1;
        public Int32 startFunction;//idx
        public Int32 blockingState;//idx
        public Int32 undefined2;
        public Int32 wantsTarget;//bool
        public Int32 targetClosest;//bool
        public Int32 randomizetargets;//bool
        public Int32 noDestructables;//bool
        public Int32 randomStartPeriod;//bool
        public Int32 dontFreeze;//bool
        public Int32 recordSpawnPoint;//bool
        public Int32 checkBusy;//bool
        public Int32 startOnAiInit;//bool
        public Int32 neverRegisterAi;//bool
        public Int32 clientOnly;//bool
        public Int32 udefined3;
        public Int32 canSeeUnsearchables;//bool
        public Int32 giveStateOnDeath;//bool
        public Int32 giveStateToUnitType;//idx
        public float giveStateRange;
        public Int32 stateToGive;//idx
    }
}
