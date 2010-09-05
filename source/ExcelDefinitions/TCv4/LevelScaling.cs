using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelScalingTCv4Row
    {
        ExcelFile.TableHeader header;

        public Int32 levelDiff;
        public Int32 PlayerAttackMonsterDmg;
        public Int32 PlayerAttackMonsterExp;
        public Int32 MonsterAttackPlayerDmg;
        public Int32 PlayerAttackPlayerDmg;
        public Int32 playerAttackPlayerKarma_tcv4;
        public Int32 PlayerAttackMonsterKarma_tcv4;
        public Int32 PlayerDamageEffectMonster_tcv4;
        public Int32 PlayerDamageEffectPlayer_tcv4;
        public Int32 MonsterDamageEffectPlayer_tcv4;
    }
}