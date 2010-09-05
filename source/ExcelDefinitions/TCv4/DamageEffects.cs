using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageEffectsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;
        [ExcelOutput(SortId = 2)]
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
        Int32 attackersProhibitingState;//idx
        Int32 defendersProhibitingState;//idx
        Int32 attackerRequiresState;//idx
        Int32 defenderRequiresState;//idx
        public Int32 sfxState;//idx
        public Int32 missileToAttach;//idx
        public Int32 fieldMissile;//idx
        public Int32 executeSkill;//idx
        Int32 executeSkillOnTarget;//idx
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
        [ExcelOutput(Exclude = true)]
        public Int32 doNotUseEffectChanceStat_tcv4;
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