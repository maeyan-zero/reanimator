using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reanimator.Excel
{
    class ItemQuality : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ItemQualityTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown01 { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] badgeFrame { get; set; }
            public Int32 code { get; set; }
            public Int32 craftingBreakdownTreasure { get; set; }
            public Int32 displayName { get; set; }
            public Int32 displayNameWithItemFormat { get; set; }
            public Int32 showBaseDesc { get; set; }
            public Int32 randomlyNamed { get; set; }
            public Int32 baseDescFormatString { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown02 { get; set; }
            public Int32 nameColor { get; set; }
            public Int32 bkgdColor { get; set; }
            public Int32 doTransactionLogging { get; set; }
            public Int32 changeItemClassToMatchRequiredQualityOnly { get; set; }
            public Int32 always_identified { get; set; }
            public float price_multiplier { get; set; }
            public float recipe_quantity_multiplier { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown03 { get; set; }
            public Int32 minLevel { get; set; }
            public Int32 rarity { get; set; }
            public Int32 vendorRarity { get; set; }
            public Int32 luckRarity { get; set; }
            public Int32 lookGroup { get; set; }
            public Int32 state { get; set; }
            public Int32 flippySound { get; set; }
            public Int32 usable { get; set; }
            public Int32 scrapQuality { get; set; }
            public Int32 isSpecialScrapQuality { get; set; }
            public Int32 scrapQualityDefault { get; set; }
            public Int32 extraScrapChance { get; set; }
            public Int32 extraScrapItem { get; set; }
            public Int32 extraScrapQuality { get; set; }
            public Int32 dismantleResultSound { get; set; }
            public Int32 downgrade { get; set; }
            public Int32 upgrade { get; set; }
            public Int32 prefixName { get; set; }
            public Int32 prefixType { get; set; }
            public Int32 prefixCount { get; set; }
            public Int32 force { get; set; }
            public Int32 suffixName { get; set; }
            public Int32 suffixType { get; set; }
            public Int32 suffixCount { get; set; }
            public float procChance { get; set; }
            public float luckProbMod { get; set; }
            public Int32 affix1Chance { get; set; }
            public Int32 affix1Type1Weight { get; set; }
            public Int32 affix1Type2Weight { get; set; }
            public Int32 affix1Type3Weight { get; set; }
            public Int32 affix1Type4Weight { get; set; }
            public Int32 affix1Type5Weight { get; set; }
            public Int32 affix1Type6Weight { get; set; }
            public Int32 affix1Type1 { get; set; }
            public Int32 affix1Type2 { get; set; }
            public Int32 affix1Type3 { get; set; }
            public Int32 affix1Type4 { get; set; }
            public Int32 affix1Type5 { get; set; }
            public Int32 affix1Type6 { get; set; }
            public Int32 affix2Chance { get; set; }
            public Int32 affix2Type1Weight { get; set; }
            public Int32 affix2Type2Weight { get; set; }
            public Int32 affix2Type3Weight { get; set; }
            public Int32 affix2Type4Weight { get; set; }
            public Int32 affix2Type5Weight { get; set; }
            public Int32 affix2Type6Weight { get; set; }
            public Int32 affix2Type1 { get; set; }
            public Int32 affix2Type2 { get; set; }
            public Int32 affix2Type3 { get; set; }
            public Int32 affix2Type4 { get; set; }
            public Int32 affix2Type5 { get; set; }
            public Int32 affix2Type6 { get; set; }
            public Int32 affix3Chance { get; set; }
            public Int32 affix3Type1Weight { get; set; }
            public Int32 affix3Type2Weight { get; set; }
            public Int32 affix3Type3Weight { get; set; }
            public Int32 affix3Type4Weight { get; set; }
            public Int32 affix3Type5Weight { get; set; }
            public Int32 affix3Type6Weight { get; set; }
            public Int32 affix3Type1 { get; set; }
            public Int32 affix3Type2 { get; set; }
            public Int32 affix3Type3 { get; set; }
            public Int32 affix3Type4 { get; set; }
            public Int32 affix3Type5 { get; set; }
            public Int32 affix3Type6 { get; set; }
            public Int32 affix4Chance { get; set; }
            public Int32 affix4Type1Weight { get; set; }
            public Int32 affix4Type2Weight { get; set; }
            public Int32 affix4Type3Weight { get; set; }
            public Int32 affix4Type4Weight { get; set; }
            public Int32 affix4Type5Weight { get; set; }
            public Int32 affix4Type6Weight { get; set; }
            public Int32 affix4Type1 { get; set; }
            public Int32 affix4Type2 { get; set; }
            public Int32 affix4Type3 { get; set; }
            public Int32 affix4Type4 { get; set; }
            public Int32 affix4Type5 { get; set; }
            public Int32 affix4Type6 { get; set; }
            public Int32 affix5Chance { get; set; }
            public Int32 affix5Type1Weight { get; set; }
            public Int32 affix5Type2Weight { get; set; }
            public Int32 affix5Type3Weight { get; set; }
            public Int32 affix5Type4Weight { get; set; }
            public Int32 affix5Type5Weight { get; set; }
            public Int32 affix5Type6Weight { get; set; }
            public Int32 affix5Type1 { get; set; }
            public Int32 affix5Type2 { get; set; }
            public Int32 affix5Type3 { get; set; }
            public Int32 affix5Type4 { get; set; }
            public Int32 affix5Type5 { get; set; }
            public Int32 affix5Type6 { get; set; }
            public Int32 affix6Chance { get; set; }
            public Int32 affix6Type1Weight { get; set; }
            public Int32 affix6Type2Weight { get; set; }
            public Int32 affix6Type3Weight { get; set; }
            public Int32 affix6Type4Weight { get; set; }
            public Int32 affix6Type5Weight { get; set; }
            public Int32 affix6Type6Weight { get; set; }
            public Int32 affix6Type1 { get; set; }
            public Int32 affix6Type2 { get; set; }
            public Int32 affix6Type3 { get; set; }
            public Int32 affix6Type4 { get; set; }
            public Int32 affix6Type5 { get; set; }
            public Int32 affix6Type6 { get; set; }
        }

        List<ItemQualityTable> itemQuality;

        public ItemQuality(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return itemQuality.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            itemQuality = ExcelTables.ReadTables<ItemsTable>(data, ref offset, Count);
        }
    }
}
