using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemQualityRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String quality;
        [ExcelOutput(SortId = 2)]
        public Int32 code;
        Int32 craftingBreakdownTreasure;
        [ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 displayName;
        [ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 displayNameWithItemFormat;
        [ExcelOutput(IsBool = true)]
        public Int32 showBaseDesc;
        Int32 randomlyNamed;
        [ExcelOutput(IsStringId = true, Table = "Strings_Items")]
        public Int32 baseDescFormatString;
        Int32 unknown01;
        [ExcelOutput(IsTableIndex = true, TableId = "FONTCOLORS", Column = "Color")]
        public Int32 nameColor;
        [ExcelOutput(IsTableIndex = true, TableId = "FONTCOLORS", Column = "Color")]
        public Int32 bkgdColor;
        [ExcelOutput(IsBool = true)]
        public Int32 doTransactionLogging;
        [ExcelOutput(IsBool = true)]
        public Int32 changeItemClassToMatchRequiredQualityOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 always_identified;
        public float price_multiplier;
        public float recipe_quantity_multiplier;
        Int32 unknown02;
        Int32 minLevel;
        public Int32 rarity;
        public Int32 vendorRarity;
        public Int32 luckRarity;
        Int32 lookGroup;
        public Int32 state;
        public Int32 flippySound;
        public Int32 usable;
        [ExcelOutput(IsTableIndex = true, TableId = "ITEM_QUALITY", Column = "quality")]
        public Int32 scrapQuality;
        public Int32 isSpecialScrapQuality;
        public Int32 scrapQualityDefault;
        public Int32 extraScrapChance;
        [ExcelOutput(IsTableIndex = true, TableId = "ITEMS", Column = "name")]
        public Int32 extraScrapItem;
        [ExcelOutput(IsTableIndex = true, TableId = "ITEM_QUALITY", Column = "quality")]
        public Int32 extraScrapQuality;
        public Int32 dismantleResultSound;
        public Int32 downgrade;
        public Int32 upgrade;
        Int32 prefixName;
        Int32 prefixType;
        Int32 prefixCount;
        Int32 force;
        Int32 suffixName;
        Int32 suffixType;
        Int32 suffixCount;
        public float procChance;
        float luckProbMod;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix1Type6Weight;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type1;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type2;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type3;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type4;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type5;
        [ExcelOutput(IsTableIndex = true, TableId = "AFFIXTYPES", Column = "AffixType")]
        public Int32 affix1Type6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix2Type6Weight;
        public Int32 affix2Type1;
        public Int32 affix2Type2;
        public Int32 affix2Type3;
        public Int32 affix2Type4;
        public Int32 affix2Type5;
        public Int32 affix2Type6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix3Type6Weight;
        public Int32 affix3Type1;
        public Int32 affix3Type2;
        public Int32 affix3Type3;
        public Int32 affix3Type4;
        public Int32 affix3Type5;
        public Int32 affix3Type6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix4Type6Weight;
        public Int32 affix4Type1;
        public Int32 affix4Type2;
        public Int32 affix4Type3;
        public Int32 affix4Type4;
        public Int32 affix4Type5;
        public Int32 affix4Type6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix5Type6Weight;
        public Int32 affix5Type1;
        public Int32 affix5Type2;
        public Int32 affix5Type3;
        public Int32 affix5Type4;
        public Int32 affix5Type5;
        public Int32 affix5Type6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Chance;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type1Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type2Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type3Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type4Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type5Weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 affix6Type6Weight;
        public Int32 affix6Type1;
        public Int32 affix6Type2;
        public Int32 affix6Type3;
        public Int32 affix6Type4;
        public Int32 affix6Type5;
        public Int32 affix6Type6;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String badgeFrame;
    }
}