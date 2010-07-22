using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DamageEffectsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string stat;

        public Int32 code;
        public Int32 damageType;
        public Int32 codeSfxDurationInMS;//intptr
        public Int32 codeSfxEffect;//intptr
        public Int32 conditional;//intptr
        public Int32 missileStats;//intptr
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
        public Int32 noRollIfParentDmgTypeSuccess;//bool
        public Int32 noRollNeeded;//bool
        public Int32 mustBeCrit;//bool
        public Int32 monsterMustDie;//bool
        public Int32 requiresNoDamage;//bool
        public Int32 doesNotRequireDamage;//bool
        public Int32 dontUseUltimateAttacker;//bool
        public Int32 dontUseSfxDefense;//bool
        public Int32 useOverrideStats;//bool
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