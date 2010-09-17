using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemQuality
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String quality;
        [ExcelAttribute(SortID = 2)]
        public Int32 code;
        public Int32 craftingBreakdownTreasure;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 displayName;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 displayNameWithItemFormat;
        [ExcelAttribute(IsBool = true)]
        public Int32 showBaseDesc;
        public Int32 randomlyNamed;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 baseDescFormatString;
        public Int32 unknown01;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "FONTCOLORS")]
        public Int32 nameColor;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "FONTCOLORS")]
        public Int32 bkgdColor;
        [ExcelAttribute(IsBool = true)]
        public Int32 doTransactionLogging;
        [ExcelAttribute(IsBool = true)]
        public Int32 changeItemClassToMatchRequiredQualityOnly;
        [ExcelAttribute(IsBool = true)]
        public Int32 always_identified;
        public float price_multiplier;
        public float recipe_quantity_multiplier;
        public Int32 unknown02;
        public Int32 minLevel;
        public Int32 rarity;
        public Int32 vendorRarity;
        public Int32 luckRarity;
        public Int32 lookGroup;
        public Int32 state;
        public Int32 flippySound;
        public Int32 usable;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ITEM_QUALITY")]
        public Int32 scrapQuality;
        public Int32 isSpecialScrapQuality;
        public Int32 scrapQualityDefault;
        public Int32 extraScrapChance;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ITEMS")]
        public Int32 extraScrapItem;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "ITEM_QUALITY")]
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
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix1Type6Weight;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affix1Type6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix2Type6Weight;
        public Int32 affix2Type1;
        public Int32 affix2Type2;
        public Int32 affix2Type3;
        public Int32 affix2Type4;
        public Int32 affix2Type5;
        public Int32 affix2Type6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix3Type6Weight;
        public Int32 affix3Type1;
        public Int32 affix3Type2;
        public Int32 affix3Type3;
        public Int32 affix3Type4;
        public Int32 affix3Type5;
        public Int32 affix3Type6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix4Type6Weight;
        public Int32 affix4Type1;
        public Int32 affix4Type2;
        public Int32 affix4Type3;
        public Int32 affix4Type4;
        public Int32 affix4Type5;
        public Int32 affix4Type6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix5Type6Weight;
        public Int32 affix5Type1;
        public Int32 affix5Type2;
        public Int32 affix5Type3;
        public Int32 affix5Type4;
        public Int32 affix5Type5;
        public Int32 affix5Type6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Chance;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Type1Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Type2Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Type3Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Type4Weight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 affix6Type5Weight;
        [ExcelAttribute(IsIntOffset = true)]
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