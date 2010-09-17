using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Affixes
    {
        TableHeader header;
        [ExcelAttribute(IsStringOffset = true, SortID = 1)]
        public Int32 affix;
        public Int32 unknown02;
        [ExcelAttribute(IsBool = true)]
        public Int32 alwaysApply;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 qualityNameString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 setNameString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 magicNameString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 replaceNameString;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 flavorText;
        public Int32 unknown03;
        public Int32 nameColor;
        public Int32 gridColor;
        public Int32 dom;
        [ExcelAttribute(SortID = 3)]
        public Int32 code;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "AFFIXTYPES")]
        public Int32 affixType6;
        public Int32 suffix;
        [ExcelAttribute(SortID = 4, SortType = "DISTINCT")]
        public Int32 group;
        public Int32 style;
        [ExcelAttribute(IsBool = true)]
        public Int32 useWhenAugmenting;
        [ExcelAttribute(IsBool = true)]
        public Int32 spawn;
        public Int32 minLevel;
        public Int32 maxLevel;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 allowTypes6;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 groupWeight;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 weight;
        public Int32 luckWeight;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Int32[] unknown04;
        public Int32 colorSet;
        public Int32 colorSetPriority;
        public Int32 state;
        public Int32 saveState;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 buyPriceMulti;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 buyPriceAdd;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 sellPriceMulti;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 sellPriceAdd;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 cond;
        public Int32 itemLevel;
        public Int32 prop1Cond;
        public Int32 prop2Cond;
        public Int32 prop3Cond;
        public Int32 prop4Cond;
        public Int32 prop5Cond;
        public Int32 prop6Cond;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property1;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property2;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property3;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property4;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property5;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 property6;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public Int32 onlyOnItemsRequiringUnitType;
    }
}
