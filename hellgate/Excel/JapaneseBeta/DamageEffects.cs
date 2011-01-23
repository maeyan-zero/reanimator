using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageEffectsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string damageEffect;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 damageType;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 codeSfxDurationInMS;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 codeSfxEffect;
        [ExcelFile.OutputAttribute(IsScript = true)]
        Int32 conditional;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 missileStats;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 invulnerableState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 attackersProhibitingState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 defendersProhibitingState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 attackerRequiresState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 defenderRequiresState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 sfxState;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 missileToAttach;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MISSILES")]
        public Int32 fieldMissile;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 executeSkill;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SKILLS")]
        public Int32 executeSkillOnTarget;//idx
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 noRollIfParentDmgTypeSuccess;
        [ExcelFile.OutputAttribute(IsBool = true)]
        Int32 noRollNeeded;
        [ExcelFile.OutputAttribute(IsBool = true)]
        Int32 mustBeCrit;
        [ExcelFile.OutputAttribute(IsBool = true)]
        Int32 monsterMustDie;
        [ExcelFile.OutputAttribute(IsBool = true)]
        Int32 requiresNoDamage;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 doesNotRequireDamage;
        [ExcelFile.OutputAttribute(IsBool = true)]
        Int32 dontUseUltimateAttacker;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 dontUseSfxDefense;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 useOverrideStats;
        public Int32 PlayerVsMonsterScalingIndex;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackLocalStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackSplashStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 attackPctCasteStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 defenseStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 effectDefenseStat;//idx
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATS")]
        public Int32 effectDefensePctStat;//idx
        public Int32 defaultDurationInTicks;
        public Int32 defaultStrength;
    }
}