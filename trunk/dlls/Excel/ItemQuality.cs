using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    class ItemQuality : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ItemQualityTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] unknown01;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] quality;
            public Int32 code;
            public Int32 craftingBreakdownTreasure;
            public Int32 displayName;
            public Int32 displayNameWithItemFormat;
            public Int32 showBaseDesc;
            public Int32 randomlyNamed;
            public Int32 baseDescFormatString;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown02;
            public Int32 nameColor;
            public Int32 bkgdColor;
            public Int32 doTransactionLogging;
            public Int32 changeItemClassToMatchRequiredQualityOnly;
            public Int32 always_identified;
            public float price_multiplier;
            public float recipe_quantity_multiplier;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] unknown03;
            public Int32 minLevel;
            public Int32 rarity;
            public Int32 vendorRarity;
            public Int32 luckRarity;
            public Int32 lookGroup;
            public Int32 state;
            public Int32 flippySound;
            public Int32 usable;
            public Int32 scrapQuality;
            public Int32 isSpecialScrapQuality;
            public Int32 scrapQualityDefault;
            public Int32 extraScrapChance;
            public Int32 extraScrapItem;
            public Int32 extraScrapQuality;
            public Int32 dismantleResultSound;
            public Int32 downgrade;
            public Int32 upgrade;
            public Int32 prefixName;
            public Int32 prefixType;
            public Int32 prefixCount;
            public Int32 force;
            public Int32 suffixName;
            public Int32 suffixType;
            public Int32 suffixCount;
            public float procChance;
            public float luckProbMod;
            public Int32 affix1Chance;
            public Int32 affix1Type1Weight;
            public Int32 affix1Type2Weight;
            public Int32 affix1Type3Weight;
            public Int32 affix1Type4Weight;
            public Int32 affix1Type5Weight;
            public Int32 affix1Type6Weight;
            public Int32 affix1Type1;
            public Int32 affix1Type2;
            public Int32 affix1Type3;
            public Int32 affix1Type4;
            public Int32 affix1Type5;
            public Int32 affix1Type6;
            public Int32 affix2Chance;
            public Int32 affix2Type1Weight;
            public Int32 affix2Type2Weight;
            public Int32 affix2Type3Weight;
            public Int32 affix2Type4Weight;
            public Int32 affix2Type5Weight;
            public Int32 affix2Type6Weight;
            public Int32 affix2Type1;
            public Int32 affix2Type2;
            public Int32 affix2Type3;
            public Int32 affix2Type4;
            public Int32 affix2Type5;
            public Int32 affix2Type6;
            public Int32 affix3Chance;
            public Int32 affix3Type1Weight;
            public Int32 affix3Type2Weight;
            public Int32 affix3Type3Weight;
            public Int32 affix3Type4Weight;
            public Int32 affix3Type5Weight;
            public Int32 affix3Type6Weight;
            public Int32 affix3Type1;
            public Int32 affix3Type2;
            public Int32 affix3Type3;
            public Int32 affix3Type4;
            public Int32 affix3Type5;
            public Int32 affix3Type6;
            public Int32 affix4Chance;
            public Int32 affix4Type1Weight;
            public Int32 affix4Type2Weight;
            public Int32 affix4Type3Weight;
            public Int32 affix4Type4Weight;
            public Int32 affix4Type5Weight;
            public Int32 affix4Type6Weight;
            public Int32 affix4Type1;
            public Int32 affix4Type2;
            public Int32 affix4Type3;
            public Int32 affix4Type4;
            public Int32 affix4Type5;
            public Int32 affix4Type6;
            public Int32 affix5Chance;
            public Int32 affix5Type1Weight;
            public Int32 affix5Type2Weight;
            public Int32 affix5Type3Weight;
            public Int32 affix5Type4Weight;
            public Int32 affix5Type5Weight;
            public Int32 affix5Type6Weight;
            public Int32 affix5Type1;
            public Int32 affix5Type2;
            public Int32 affix5Type3;
            public Int32 affix5Type4;
            public Int32 affix5Type5;
            public Int32 affix5Type6;
            public Int32 affix6Chance;
            public Int32 affix6Type1Weight;
            public Int32 affix6Type2Weight;
            public Int32 affix6Type3Weight;
            public Int32 affix6Type4Weight;
            public Int32 affix6Type5Weight;
            public Int32 affix6Type6Weight;
            public Int32 affix6Type1;
            public Int32 affix6Type2;
            public Int32 affix6Type3;
            public Int32 affix6Type4;
            public Int32 affix6Type5;
            public Int32 affix6Type6;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            public byte[] badgeFrame;
        }

        public ItemQuality(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ItemQualityTable>(data, ref offset, Count);
        }
    }
}
