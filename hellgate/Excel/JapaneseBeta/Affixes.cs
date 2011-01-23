using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixesBeta
    {
        RowHeader header;

        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 affix;
        public Int32 unknown02;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table C8
        public Int32 appear0;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear1;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear4;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear5;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear6;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear7;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear8;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]
        public Int32 appear9;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysApply;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 qualityNameString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 setNameString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 magicNameString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 replaceNameString;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 flavorText;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_LOOK_GROUPS")]
        public Int32 lookGroup;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 nameColor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 gridColor;
        public Int32 dom;
        [ExcelOutput(SortColumnOrder = 3)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES", SortColumnOrder = 2, SecondarySortColumn = "code")]
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
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType11;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType13;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType14;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType15;
        [ExcelOutput(IsBool = true)]
        public Int32 suffix;
        [ExcelOutput(SortColumnOrder = 4, IsSecondaryString = true)]
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
		public Int32 minSetItems;
        [ExcelOutput(IsScript = true)]
        public Int32 groupWeight;
        [ExcelOutput(IsScript = true)]
        public Int32 weight;
        public Int32 weightLuck;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Int32[] unknown04;
        [ExcelOutput(IsTableIndex = true, TableStringId = "COLORSETS")]
        public Int32 colorSet;
        public Int32 colorSetPriority;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;
        [ExcelOutput(IsBool = true)]
        public Int32 saveState;
        [ExcelOutput(IsScript = true)]
        public Int32 buyPriceMult;
        [ExcelOutput(IsScript = true)]
        public Int32 buyPriceAdd;
        [ExcelOutput(IsScript = true)]
        public Int32 sellPriceMult;
        [ExcelOutput(IsScript = true)]
        public Int32 sellPriceAdd;
        [ExcelOutput(IsScript = true)]
        public Int32 cond;
        public Int32 itemLevel;
        [ExcelOutput(IsScript = true)]
        public Int32 prop1ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop2ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop3ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop4ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop5ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop6ConstrainCondition;
        [ExcelOutput(IsScript = true)]
        public Int32 prop1Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 prop2Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 prop3Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 prop4Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 prop5Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 prop6Cond;
        [ExcelOutput(IsScript = true)]
        public Int32 property1;
        [ExcelOutput(IsScript = true)]
        public Int32 property2;
        [ExcelOutput(IsScript = true)]
        public Int32 property3;
        [ExcelOutput(IsScript = true)]
        public Int32 property4;
        [ExcelOutput(IsScript = true)]
        public Int32 property5;
        [ExcelOutput(IsScript = true)]
        public Int32 property6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyOnItemsRequiringUnitType;
    }
}