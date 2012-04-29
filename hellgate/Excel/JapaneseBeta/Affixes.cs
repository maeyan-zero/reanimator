using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixesBeta
    {
        ExcelFile.RowHeader header;

        [ExcelFile.OutputAttribute(IsStringOffset = true, SortColumnOrder = 1)]
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
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 alwaysApply;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 qualityNameString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 setNameString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 magicNameString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 replaceNameString;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 flavorText;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEM_LOOK_GROUPS")]
        public Int32 lookGroup;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 nameColor;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 gridColor;
        public Int32 dom;
        [ExcelFile.OutputAttribute(SortColumnOrder = 3)]
        public Int32 code;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES", SortColumnOrder = 2, SecondarySortColumn = "code")]
        public Int32 affixType1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType7;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType8;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType9;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType10;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType11;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType12;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType13;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType14;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 affixType15;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 suffix;
        [ExcelFile.OutputAttribute(SortColumnOrder = 4, IsSecondaryString = true)]
        public Int32 group;
        public Style style;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 useWhenAugmenting;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 spawn;
        public Int32 minLevel;
        public Int32 maxLevel;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowTypes6;
		public Int32 minSetItems;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 groupWeight;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 weight;
        public Int32 weightLuck;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        Int32[] unknown04;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "COLORSETS")]
        public Int32 colorSet;
        public Int32 colorSetPriority;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 saveState;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 buyPriceMult;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 buyPriceAdd;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 sellPriceMult;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 sellPriceAdd;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 cond;
        public Int32 itemLevel;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop1ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop2ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop3ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop4ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop5ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop6ConstrainCondition;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop1Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop2Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop3Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop4Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop5Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 prop6Cond;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property1;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property2;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property3;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property4;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property5;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 property6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyOnItemsRequiringUnitType;

        public enum Style
        {
            Null = -1,
            Stat = 0,
            Proc = 1
        }
    }
}