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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] TCv4_1;
    }
}