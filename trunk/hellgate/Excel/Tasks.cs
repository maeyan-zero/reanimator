using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Tasks
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 rarity;
        [ExcelOutput(IsBool = true)]
        public Int32 hostileAreaOnly;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 accessibleAreaOnly;//bool;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 timeLimitInMinutes;//intptr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 exterminateCount;//intptr
        [ExcelOutput(IsIntOffset = true)]
        public Int32 triggerPercent;//intptr
        public Int32 objectClass;//idx;
        [ExcelOutput(IsBool = true)]
        public Int32 canSave;//bool
        Int32 undefined1;
        [ExcelOutput(IsBool = true)]
        public Int32 doNotOfferSimilarTasks;//bool
        public Int32 nameStringKey;//stridx
        public Int32 descriptionDialog;//idx
        public Int32 completedDialog;//idx
        public Int32 inCompleteDialog;//idx
        public Int32 numRewardTakes;
        public Int32 treasureClassReward;//idx
        public Int32 treasureClassCollect;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 collectModdedToRewards;//bool;
        public Int32 minSlotsOnReward;
        [ExcelOutput(IsBool = true)]
        public Int32 fillAllRewardSlots;//bool;
        public Int32 filledSlotsOnForgeReward;
        [ExcelOutput(IsBool = true)]
        public Int32 implemented;//bool;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string createFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined2;
    }
}
