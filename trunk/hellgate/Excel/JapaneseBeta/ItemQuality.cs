using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemQualityBeta
    {
        RowHeader header;
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
		public Int32 keeppedUnitToDatabaseByDelete
        [ExcelOutput(IsBool = true)]
        public Int32 changeItemClassToMatchRequiredQualityOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 always_identified;
        public float price_multiplier;
        public float recipe_quantity_multiplier;
        public Int32 unknown02;
        public Int32 minLevel;
        public Int32 rarity;
        public Int32 nightmareRarity;
        public Int32 hellRarity;
        public Int32 vendorRarity;
        public Int32 luckRarity;		
        public Int32 gamblingRarity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_LOOK_GROUP")]
        public Int32 lookGroup;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 flippySound;
        public Int32 usable;
        public Int32 feedWeight;
        public Int32 successRate;
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
		public Int32 qualityLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 prefixName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 prefixType;
        [ExcelOutput(IsScript = true)]
        public Int32 prefixCount;
        [ExcelOutput(IsBool = true)]
        public Int32 force;
        [ExcelOutput(IsBool = true)]
        public Int32 suffixName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 suffixType;
        [ExcelOutput(IsScript = true)]
        public Int32 suffixCount;
        [ExcelOutput(IsBool = true)]
        public Int32 defaultBindability;
		public Int32 requiredUnbindItem;
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
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table 44
        public Int32 nAffixPick;
        [ExcelOutput(IsBool = true)]
        public Int32 setItem;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table 44
        public Int32 nSetAffixPick;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String badgeFrame;
        [ExcelOutput(IsBool = true)]
        public Int32 allowItemPickupMessage;
    }
}
