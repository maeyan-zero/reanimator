using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Excel
{
    class Affixes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public class AffixesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            byte[] unknown01;
            public Int32 affix { get; set; }
            public Int32 unknown02 { get; set; }
            public Int32 alwaysApply { get; set; }
            public Int32 qualityNameString { get; set; }
            public Int32 setNameString { get; set; }
            public Int32 magicNameString { get; set; }
            public Int32 replaceNameString { get; set; }
            public Int32 flavorText { get; set; }
            public Int32 unknown03 { get; set; }
            public Int32 nameColor { get; set; }
            public Int32 gridColor { get; set; }
            public Int32 dom { get; set; }
            public Int32 code { get; set; }
            public Int32 affixType1 { get; set; }
            public Int32 affixType2 { get; set; }
            public Int32 affixType3 { get; set; }
            public Int32 affixType4 { get; set; }
            public Int32 affixType5 { get; set; }
            public Int32 affixType6 { get; set; }
            public Int32 suffix { get; set; }
            public Int32 group { get; set; }
            public Int32 style { get; set; }
            public Int32 useWhenAugmenting { get; set; }
            public Int32 spawn { get; set; }
            public Int32 minLevel { get; set; }
            public Int32 maxLevel { get; set; }
            public Int32 allowTypes1 { get; set; }
            public Int32 allowTypes2 { get; set; }
            public Int32 allowTypes3 { get; set; }
            public Int32 allowTypes4 { get; set; }
            public Int32 allowTypes5 { get; set; }
            public Int32 allowTypes6 { get; set; }
            public Int32 groupWeight { get; set; }
            public Int32 weight { get; set; }
            public Int32 luckWeight { get; set; }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] unknown04;
            public Int32 colorSet { get; set; }
            public Int32 colorSetPriority { get; set; }
            public Int32 state { get; set; }
            public Int32 saveState { get; set; }
            public Int32 buyPriceMulti { get; set; }
            public Int32 buyPriceAdd { get; set; }
            public Int32 sellPriceMulti { get; set; }
            public Int32 cond { get; set; }
            public Int32 itemLevel { get; set; }
            public Int32 prop1Cond { get; set; }
            public Int32 prop2Cond { get; set; }
            public Int32 prop3Cond { get; set; }
            public Int32 prop4Cond { get; set; }
            public Int32 prop5Cond { get; set; }
            public Int32 prop6Cond { get; set; }
            public Int32 property1 { get; set; }
            public Int32 property2 { get; set; }
            public Int32 property3 { get; set; }
            public Int32 property4 { get; set; }
            public Int32 property5 { get; set; }
            public Int32 property6 { get; set; }
            public Int32 onlyOnItemsRequiringUnitType { get; set; }
        }

        List<AffixesTable> affixes;

        public Affixes(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return affixes.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            affixes = ExcelTables.ReadTables<ItemsTable>(data, ref offset, Count);
        }
    }
}
