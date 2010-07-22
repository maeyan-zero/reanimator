using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TasksRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        public Int32 rarity;
        public Int32 hostileAreaOnly;//bool;
        public Int32 accessibleAreaOnly;//bool;
        public Int32 timeLimitInMinutes;//intptr
        public Int32 exterminateCount;//intptr
        public Int32 triggerPercent;//intptr
        public Int32 objectClass;//idx;
        public Int32 canSave;//bool
        public Int32 undefined1;
        public Int32 doNotOfferSimilarTasks;//bool
        public Int32 nameStringKey;//stridx
        public Int32 descriptionDialog;//idx
        public Int32 completedDialog;//idx
        public Int32 inCompleteDialog;//idx
        public Int32 numRewardTakes;
        public Int32 treasureClassReward;//idx
        public Int32 treasureClassCollect;//idx
        public Int32 collectModdedToRewards;//bool;
        public Int32 minSlotsOnReward;
        public Int32 fillAllRewardSlots;//bool;
        public Int32 filledSlotsOnForgeReward;
        public Int32 implemented;//bool;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string createFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined2;
    }
}