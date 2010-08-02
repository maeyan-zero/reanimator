using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    // TODO: what are SortId 2 & 4?
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(IsStringOffset = true, SortId = 1)]
        public Int32 affix;
        public Int32 unknown02;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysApply;
        [ExcelOutput(IsStringId = true, Table = "Strings_Affix")]
        public Int32 qualityNameString;
        [ExcelOutput(IsStringId = true, Table = "Strings_Affix")]
        public Int32 setNameString;
        [ExcelOutput(IsStringId = true, Table = "Strings_Affix")]
        public Int32 magicNameString;
        [ExcelOutput(IsStringId = true, Table = "Strings_Affix")]
        public Int32 replaceNameString;
        [ExcelOutput(IsStringId = true, Table = "Strings_Affix")]
        public Int32 flavorText;
        public Int32 unknown03;
        public Int32 nameColor;
        public Int32 gridColor;
        public Int32 dom;
        [ExcelOutput(SortId = 3)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableId = 0x7530, Column = "AffixType")]
        public Int32 affixType1;
        public Int32 affixType2;
        public Int32 affixType3;
        public Int32 affixType4;
        public Int32 affixType5;
        public Int32 affixType6;
        public Int32 suffix;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 group;
        public Int32 style;
        public Int32 useWhenAugmenting;
        public Int32 spawn;
        public Int32 minLevel;
        public Int32 maxLevel;
        public Int32 allowTypes1;
        public Int32 allowTypes2;
        public Int32 allowTypes3;
        public Int32 allowTypes4;
        public Int32 allowTypes5;
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
        public Int32 buyPriceMulti;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 buyPriceAdd;
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
        public Int32 onlyOnItemsRequiringUnitType;
    }
}
