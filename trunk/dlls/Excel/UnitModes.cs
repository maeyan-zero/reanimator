using System;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class UnitModes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UnitModesTable
        {
            TableHeader header;

            [ExcelOutput(IsStringOffset = true)]
            public Int32 mode;//pchar
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            byte[] undefined1;
            public Int32 code;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public Int32[] block;
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
            public Int32 forceClear;//bool
            public Int32 clearAi;//bool
            public Int32 clearSkill;//bool
            public Int32 clearState;//idx
            public Int32 clearStateEnd;//idx
            public Int32 doEvent;//bool
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
            public Int32[] undefined4;
            public Int32 endMode;//idx
            public Int32 velocityName;//idx?
            public Int32 velocityPriority;
            public Int32 animPriority;
            public Int32 loadPriorityPercent;
            public Int32 velocityUsesMeleeSpeed;//bool
            public Int32 velocityChangedByStats;//bool
            public Int32 noLoop;//bool
            public Int32 restoreAi;//bool
            public Int32 steps;//bool
            public Int32 lazyEndForControlUnit;//bool
            public Int32 noAnimation;//bool
            public Int32 ragdoll;//bool
            public Int32 playRight;//bool
            public Int32 playLeft;//bool
            public Int32 playTorso;//bool
            public Int32 playLegs;//bool
            public Int32 playJustDefault;//bool
            public Int32 isAggressive;//bool
            public Int32 playAllVariations;//bool
            public Int32 resetMixableOnStart;//bool
            public Int32 resetMixableOnEnd;//bool
            public Int32 randStartTime;//bool
            public Int32 durationFromAnims;//bool
            public Int32 durationFromContact;//bool
            public Int32 clearAdjustStance;//bool
            public Int32 checkCanInterrupt;//bool
            public Int32 useBackupModeAnims;//bool
            public Int32 playOnInventoryModel;//bool
            public Int32 undefined5;


        }

        public UnitModes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<UnitModesTable>(data, ref offset, Count);
        }
    }
}
