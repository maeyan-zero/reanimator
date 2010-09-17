using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageEffects
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String stat;
        [ExcelAttribute(SortID = 2)]
        public Int32 code;
        public Int32 damageType;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 codeSfxDurationInMS;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 codeSfxEffect;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 conditional;
        [ExcelAttribute(IsIntOffset = true)]
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
        [ExcelAttribute(IsBool = true)]
        public Int32 noRollIfParentDmgTypeSuccess;
        [ExcelAttribute(IsBool = true)]
        public Int32 noRollNeeded;
        [ExcelAttribute(IsBool = true)]
        public Int32 mustBeCrit;
        [ExcelAttribute(IsBool = true)]
        public Int32 monsterMustDie;
        [ExcelAttribute(IsBool = true)]
        public Int32 requiresNoDamage;
        [ExcelAttribute(IsBool = true)]
        public Int32 doesNotRequireDamage;
        [ExcelAttribute(IsBool = true)]
        public Int32 dontUseUltimateAttacker;
        [ExcelAttribute(IsBool = true)]
        public Int32 dontUseSfxDefense;
        [ExcelAttribute(IsBool = true)]
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