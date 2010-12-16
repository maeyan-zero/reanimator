using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Recipes
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipe;
        public Int32 String;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 cubeRecipe;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysKnown;
        [ExcelOutput(IsBool = true)]
        public Int32 dontRequireExactIngredients;
        [ExcelOutput(IsBool = true)]
        public Int32 allowInRandomSingleUse;
        [ExcelOutput(IsBool = true)]
        public Int32 removeOnLoad;
        public Int32 weight;
        public Int32 experienceEarned;
        public Int32 goldReward;
        public Int32 resultQualityModifiesIngredientQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient1ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient1UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient1ItemQuality;
        public Int32 ingredient1MinQuantity;
        public Int32 ingredient1MaxQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient2ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient2UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient2ItemQuality;
        public Int32 ingredient2MinQuantity;
        public Int32 ingredient2MaxQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient3ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient3UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient3ItemQuality;
        public Int32 ingredient3MinQuantity;
        public Int32 ingredient3MaxQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient4ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient4UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient4ItemQuality;
        public Int32 ingredient4MinQuantity;
        public Int32 ingredient4MaxQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient5ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient5UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient5ItemQuality;
        public Int32 ingredient5MinQuantity;
        public Int32 ingredient5MaxQuantity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 ingredient6ItemClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 ingredient6UnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_QUALITY")]
        public Int32 ingredient6ItemQuality;
        public Int32 ingredient6MinQuantity;
        public Int32 ingredient6MaxQuantity;
        public Int32 craftResult1;
        public Int32 craftResult2;
        public Int32 craftResult3;
        public Int32 craftResult4;
        public Int32 craftResult5;
        public Int32 craftResult6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasureResult1;
        public Int32 treasureResult2;
        public Int32 treasureResult3;
        public Int32 treasureResult4;
        public Int32 treasureResult5;
        public Int32 treasureResult6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOC")]
        public Int32 mustPlaceInInvSlot;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask;
        public Int32 spawnLevelMin;
        public Int32 spawnLevelMax;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            canBeLearned = (1 << 0),
            canSpawn = (1 << 1)
        }
    }
}