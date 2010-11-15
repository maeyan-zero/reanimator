using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;
using TableHeader = Reanimator.ExcelFile.TableHeader;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixesRow
    {
        TableHeader header;
        [ExcelOutput(IsStringOffset = true, SortAscendingID = 1)]
        public Int32 affix;
        public Int32 unknown02;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysApply;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Affix")]
        public Int32 qualityNameString;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Affix")]
        public Int32 setNameString;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Affix")]
        public Int32 magicNameString;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Affix")]
        public Int32 replaceNameString;
        [ExcelOutput(IsStringID = true, TableStringId = "Strings_Affix")]
        public Int32 flavorText;
        public Int32 unknown03;
        public Int32 nameColor;
        public Int32 gridColor;
        public Int32 dom;
        [ExcelOutput(SortPostOrderID = 2, SortAscendingID = 3)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType6;
        public Int32 suffix;
        [ExcelOutput(IsSecondaryString = true, SortDistinctID = 4)]
        public Int32 group;
        public Int32 style;
        [ExcelOutput(IsBool = true)]
        public Int32 useWhenAugmenting;
        [ExcelOutput(IsBool = true)]
        public Int32 spawn;
        public Int32 minLevel;
        public Int32 maxLevel;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes6;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 groupWeight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 weight;
        public Int32 luckWeight;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Int32[] unknown04;
        public Int32 colorSet;
        public Int32 colorSetPriority;
        public Int32 state;
        public Int32 saveState;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 buyPriceMulti;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 buyPriceAdd;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 sellPriceMulti;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 sellPriceAdd;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 cond;
        public Int32 itemLevel;
        public Int32 prop1Cond;
        public Int32 prop2Cond;
        public Int32 prop3Cond;
        public Int32 prop4Cond;
        public Int32 prop5Cond;
        public Int32 prop6Cond;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property1;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property3;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property5;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 property6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyOnItemsRequiringUnitType;
    }
}
