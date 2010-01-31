using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ItemLevels : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class ItemLevelsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown;
            public Int32 level { get; set; }
            public Int32 baseDamageMultiplyer { get; set; }
            public Int32 armor { get; set; }
            public Int32 armorBuffer { get; set; }
            public Int32 armorRegen { get; set; }
            public Int32 shields { get; set; }
            public Int32 shieldsBuffer { get; set; }
            public Int32 shieldsRegen { get; set; }
            public Int32 sfxAttackAbility { get; set; }
            public Int32 sfxDefenceAbility { get; set; }
            public Int32 interruptAttack { get; set; }
            public Int32 interruptDefence { get; set; }
            public Int32 stealthAttack { get; set; }
            public Int32 aiChangerAttack { get; set; }
            public Int32 feed { get; set; }
            public Int32 levelRequirement { get; set; }
            public Int32 itemLevelMin { get; set; }
            public Int32 itemLevelMax { get; set; }
            public Int32 buyPriceBase { get; set; }
            public Int32 sellPriceBase { get; set; }
            public Int32 augmentCostCommon { get; set; }
            public Int32 augmentCostRare { get; set; }
            public Int32 augmentCostLegendary { get; set; }
            public Int32 augmentCostRandom { get; set; }
            public Int32 scrapUpgradeQuantity { get; set; }
            public Int32 specialScrapUpgradeQuantity { get; set; }
            public Int32 itemLevelsPerUpgrade { get; set; }
        }

        List<ItemLevelsTable> itemLevels;

        public ItemLevels(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return itemLevels.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            itemLevels = ExcelTables.ReadTables<ItemLevelsTable>(data, ref offset, Count);
        }
    }
}
