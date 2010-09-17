using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitModesRow
    {
        ExcelFile.TableHeader header;

        [ExcelFile.ExcelOutputAttribute(IsStringOffset = true, SortId = 1)]
        public Int32 mode;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined1;
        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Int32[] block;
        [ExcelOutput(IsBool = true)]
        public Int32 blockOnGround;//bool
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Int32[] wait;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Int32[] clear;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public Int32[] endBlock;
        public Int32 group;//idx
        public Int32 otherhand;//idx
        public Int32 backUp;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 forceClear;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clearAi;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clearSkill;//bool
        public Int32 clearState;//idx
        public Int32 clearStateEnd;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 doEvent;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 endEvent;//bool
        [ExcelFile.ExcelOutputAttribute(IsStringOffset = true)]
        public Int32 doFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined2;
        [ExcelFile.ExcelOutputAttribute(IsStringOffset = true)]
        public Int32 clearFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined3;
        [ExcelFile.ExcelOutputAttribute(IsStringOffset = true)]
        public Int32 endFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined4;
        public Int32 endMode;//idx
        public Int32 velocityName;//idx?
        public Int32 velocityPriority;
        public Int32 animPriority;
        public Int32 loadPriorityPercent;
        [ExcelOutput(IsBool = true)]
        public Int32 velocityUsesMeleeSpeed;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 velocityChangedByStats;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 noLoop;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 restoreAi;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 steps;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 lazyEndForControlUnit;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 noAnimation;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 ragdoll;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playRight;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playLeft;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playTorso;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playLegs;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playJustDefault;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isAggressive;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playAllVariations;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 resetMixableOnStart;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 resetMixableOnEnd;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 randStartTime;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 durationFromAnims;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 durationFromContact;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clearAdjustStance;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 checkCanInterrupt;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 useBackupModeAnims;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 playOnInventoryModel;//bool
        public Int32 undefined5;
    }
}