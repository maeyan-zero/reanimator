using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TestCentre
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitModesTCv4
    {
        RowHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 mode;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined1;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")] // x256 of these
        public Int32 block1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 block2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 block3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 block4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 block5;
		[ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 251)]
        Int32[] block;
        [ExcelOutput(IsBool = true)]
        public Int32 blockOnGround;//bool
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")] // x256 of these
        public Int32 wait1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 wait2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 254)]
        Int32[] wait;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")] // x256 of these
        public Int32 clear1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 clear9;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 247)]
        Int32[] clear;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")] // x256 of these
        public Int32 endBlock1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 endBlock2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 endBlock3;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 253)]
        Int32[] endBlock;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODE_GROUPS")]
        public Int32 group;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 otherhand;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 backUp;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 forceClear;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clearAi;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 clearSkill;//bool
        public Int32 setstateformode_tcv4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 clearState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 clearStateEnd;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 doEvent;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 endEvent;//bool
        [ExcelOutput(IsStringOffset = true)]
        public Int32 doFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int32[] undefined2;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 clearFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int32[] undefined3;
        [ExcelOutput(IsStringOffset = true)]
        public Int32 endFunction;//pchar
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 endMode;//idx
        public Int32 velocityName; // XLS_InternalIndex_VelocityName (XLS_UNITMODE_DATA+3DB), 0x09
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
        public Int32 hideWeapons_tcv4;
        public Int32 emoteAllowedHellgate_tcv4;
        public Int32 emoteAllowedMythos_tcv4;
        public Int32 undefined5;
    }
}