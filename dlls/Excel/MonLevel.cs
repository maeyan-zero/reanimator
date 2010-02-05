using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MonLevel : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MonLevelTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            public Int32 level;
            public Int32 hitPoints;
            public Int32 experience;
            public Int32 damage;
            public Int32 attackRating;
            public Int32 armor;
            public Int32 armorBuffer;
            public Int32 armorRegen;
            public Int32 toHitBonus;
            public Int32 shield;
            public Int32 shieldBuffer;
            public Int32 shieldRegen;
            public Int32 sfxAttack;
            public Int32 sfxStrengthPercent;
            public Int32 sfxDefense;
            public Int32 interruptAttack;
            public Int32 interruptDefense;
            public Int32 stealthDefense;
            public Int32 aiChangeAttack;
            public Int32 aiChangeDefense;
            public Int32 corpseExplodePoints;
            public Int32 moneyChance;
            public Int32 moneyMin;
            public Int32 moneyDelta;
            public Int32 dexterity;
            public Int32 strength;
            public Int32 statPoints;
            public Int32 itemLevel;//intptr
        }

        public MonLevel(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MonLevelTable>(data, ref offset, Count);
        }
    }
}
