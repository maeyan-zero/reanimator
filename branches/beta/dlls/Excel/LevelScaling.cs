using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LevelScaling : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelScalingTable
        {
            TableHeader header;

            public Int32 levelDiff;
            public Int32 PlayerAttackMonsterDmg;
            public Int32 PlayerAttackMonsterExp;
            public Int32 MonsterAttackPlayerDmg;
            public Int32 PlayerAttackPlayerDmg;
        }

        public LevelScaling(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelScalingTable>(data, ref offset, Count);
        }
    }
}
