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
        struct ItemLevelsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown;
            Int32 level;
            Int32 baseDamageMultiplyer;
            Int32 armor;
            Int32 armorBuffer;
            Int32 armorRegen;
            Int32 shields;
            Int32 shieldsBuffer;
            Int32 shieldsRegen;
            Int32 sfxAttackAbility;
            Int32 sfxDefenceAbility;
            Int32 interruptAttack;
            Int32 interruptDefence;
            Int32 stealthAttack;
            Int32 aiChangerAttack;
            Int32 feed;
            Int32 levelRequirement;
            Int32 itemLevelMin;
            Int32 itemLevelMax;
            Int32 buyPriceBase;
            Int32 sellPriceBase;
            Int32 augmentCostCommon;
            Int32 augmentCostRare;
            Int32 augmentCostLegendary;
            Int32 augmentCostRandom;
            Int32 scrapUpgradeQuantity;
            Int32 specialScrapUpgradeQuantity;
            Int32 itemLevelsPerUpgrade;
        }

        List<ItemLevelsTable> itemLevels;

        public ItemLevels(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            itemLevels = ExcelTables.ReadTables<ItemLevelsTable>(data, ref offset, Count);
        }
    }
}
