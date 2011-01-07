using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemQualityTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String quality;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 craftingBreakdownTreasure;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayName;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayNameWithItemFormat;
        [ExcelOutput(IsBool = true)]
        public Int32 showBaseDesc;
        public Int32 randomlyNamed;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 baseDescFormatString;
        public Int32 unknown01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 nameColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 bkgdColor;
        [ExcelOutput(IsBool = true)]
        public Int32 doTransactionLogging;
        [ExcelOutput(IsBool = true)]
        public Int32 changeItemClassToMatchRequiredQualityOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 always_identified;
        public float price_multiplier;
        public float recipe_quantity_multiplier;
        public Int32 unknown02;
        public Int32 minLevel;
        public Int32 rarity;
        public Int32 vendorRarity;
        public Int32 luckRarity;
        public Int32 gamblingRarity_tcv4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_LOOK_GROUP")]
        public Int32 lookGroup;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 flippySound;
        public Int32 usable;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 scrapQuality;
        public Int32 isSpecialScrapQuality;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 scrapQualityDefault;
        public Int32 extraScrapChance;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 extraScrapItem;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 extraScrapQuality;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 dismantleResultSound;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 downgrade;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 upgrade;
        public Int32 qualityLevel_tcv4;
        public Int32 prefixName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 prefixType;
        public Int32 prefixCount;
        public Int32 force;
        public Int32 suffixName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 suffixType;
        public Int32 suffixCount;
        public float procChance;
        public float luckProbMod;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix1Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix1Type6;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix2Type6Weight;
        public Int32 affix2Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix2Type6;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix3Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix3Type6;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix4Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix4Type6;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix5Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix5Type6;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Chance;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type1Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type2Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type3Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type4Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type5Weight;
        [ExcelOutput(IsScript = true)]
        public Int32 affix6Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affix6Type6;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String badgeFrame;
        public Int32 craftingAllowsQuality_tcv4;
    }
}