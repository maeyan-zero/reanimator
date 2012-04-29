using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RecipesTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipe;
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
        public Int32 resultIsUnidentified_tcv4;
        public Int32 weight;
        public Int32 experienceEarned;
        public Int32 goldReward;
        public Int32 resultQualityModifiesIngredientQuantity;
        public Int32 resultLevelSameAsIngredient1Level_tcv4;
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
        [ExcelOutput(IsTableIndex = true, TableStringId = "INVLOCIDX")]
        public Int32 mustPlaceInInvSlot;
        [ExcelOutput(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask;
        public Int32 spawnLevelMin;
        public Int32 spawnLevelMax;
        public Int32 recipeCategoriesRequired1_tcv4;//( 0 to 3 )
        public Int32 recipeCategoriesRequired2_tcv4;
        public Int32 recipeCategoriesRequired3_tcv4;
        public Int32 recipeCategoryLevelRequired1_tcv4;//( 0 to 3 )
        public Int32 recipeCategoryLevelRequired2_tcv4;
        public Int32 recipeCategoryLevelRequired3_tcv4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 144)]
        Int32[] undefined_tcv4;
        public Int32 baseCost_tcv4;
        public Int32 propertyScript_tcv4;
        public Int32 spawnForOnlyUnitType1_tcv4;
        public Int32 spawnForOnlyUnitType2_tcv4;
        public Int32 spawnForOnlyUnitType3_tcv4;
        public Int32 spawnForOnlyUnitType4_tcv4;
        public Int32 spawnForOnlyUnitType5_tcv4;
        public Int32 spawnForOnlyUnitType6_tcv4;
        public Int32 spawnForOnlyUnitType7_tcv4;
        public Int32 spawnForOnlyUnitType8_tcv4;
        public Int32 spawnForOnlyUnitType9_tcv4;
        public Int32 spawnForOnlyUnitType10_tcv4;
        public Int32 spawnForOnlyMonsterClass1_tcv4;
        public Int32 spawnForOnlyMonsterClass2_tcv4;
        public Int32 spawnForOnlyMonsterClass3_tcv4;
        public Int32 spawnForOnlyMonsterClass4_tcv4;
        public Int32 spawnForOnlyMonsterClass5_tcv4;
        public Int32 spawnForOnlyMonsterClass6_tcv4;
        public Int32 spawnForOnlyMonsterClass7_tcv4;
        public Int32 spawnForOnlyMonsterClass8_tcv4;
        public Int32 spawnForOnlyMonsterClass9_tcv4;
        public Int32 spawnForOnlyMonsterClass10_tcv4;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            canBeLearned = (1 << 0),
            canSpawn = (1 << 1),
            spawnAtMerchant_tcv4 = (1 << 2)
        }
    }
}