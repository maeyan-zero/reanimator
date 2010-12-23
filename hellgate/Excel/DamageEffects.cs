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
        [ExcelOutput(IsIntOffset = true)]
        public Int32 codeSfxDurationInMS;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 codeSfxEffect;
        [ExcelOutput(IsIntOffset = true)]
        Int32 conditional;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 missileStats;
        public Int32 invulnerableState;//idx
        public Int32 attackersProhibitingState;//idx
        public Int32 defendersProhibitingState;//idx
        public Int32 attackerRequiresState;//idx
        public Int32 defenderRequiresState;//idx
        public Int32 sfxState;//idx
        public Int32 missileToAttach;//idx
        public Int32 fieldMissile;//idx
        public Int32 executeSkill;//idx
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
        public Int32 attackStat;//idx
        public Int32 attackLocalStat;//idx
        public Int32 attackSplashStat;//idx
        public Int32 attackPctStat;//idx
        public Int32 attackPctLocalStat;//idx
        public Int32 attackPctSplashStat;//idx
        public Int32 attackPctCasteStat;//idx
        public Int32 defenseStat;//idx
        public Int32 effectDefenseStat;//idx
        public Int32 effectDefensePctStat;//idx
        public Int32 defaultDurationInTicks;
        public Int32 defaultStrength;
    }
}