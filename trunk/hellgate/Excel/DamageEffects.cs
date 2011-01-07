using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageEffects
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 damageType;
        [ExcelOutput(IsScript = true)]
        public Int32 codeSfxDurationInMS;
        [ExcelOutput(IsScript = true)]
        public Int32 codeSfxEffect;
        [ExcelOutput(IsScript = true)]
        Int32 conditional;
        [ExcelOutput(IsScript = true)]
        public Int32 missileStats;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 invulnerableState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 attackersProhibitingState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 defendersProhibitingState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 attackerRequiresState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 defenderRequiresState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 sfxState;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 missileToAttach;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 executeSkill;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 executeSkillOnTarget;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 noRollIfParentDmgTypeSuccess;
        [ExcelOutput(IsBool = true)]
        Int32 noRollNeeded;
        [ExcelOutput(IsBool = true)]
        Int32 mustBeCrit;
        [ExcelOutput(IsBool = true)]
        Int32 monsterMustDie;
        [ExcelOutput(IsBool = true)]
        Int32 requiresNoDamage;
        [ExcelOutput(IsBool = true)]
        public Int32 doesNotRequireDamage;
        [ExcelOutput(IsBool = true)]
        Int32 dontUseUltimateAttacker;
        [ExcelOutput(IsBool = true)]
        public Int32 dontUseSfxDefense;
        [ExcelOutput(IsBool = true)]
        public Int32 useOverrideStats;
        public Int32 PlayerVsMonsterScalingIndex;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackLocalStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackSplashStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctLocalStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctSplashStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctCasteStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 defenseStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 effectDefenseStat;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 effectDefensePctStat;//idx
        public Int32 defaultDurationInTicks;
        public Int32 defaultStrength;
    }
}