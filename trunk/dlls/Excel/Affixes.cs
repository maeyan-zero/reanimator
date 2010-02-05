using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    class Affixes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AffixesTable
        {
            TableHeader header;
            public Int32 affix;
            public Int32 unknown02;
            public Int32 alwaysApply;
            public Int32 qualityNameString;
            public Int32 setNameString;
            public Int32 magicNameString;
            public Int32 replaceNameString;
            public Int32 flavorText;
            public Int32 unknown03;
            public Int32 nameColor;
            public Int32 gridColor;
            public Int32 dom;
            public Int32 code;
            public Int32 affixType1;
            public Int32 affixType2;
            public Int32 affixType3;
            public Int32 affixType4;
            public Int32 affixType5;
            public Int32 affixType6;
            public Int32 suffix;
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
            public Int32 groupWeight;
            public Int32 weight;
            public Int32 luckWeight;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] unknown04;
            public Int32 colorSet;
            public Int32 colorSetPriority;
            public Int32 state;
            public Int32 saveState;
            public Int32 buyPriceMulti;
            public Int32 buyPriceAdd;
            public Int32 sellPriceMulti;
            public Int32 sellPriceAdd;
            public Int32 cond;
            public Int32 itemLevel;
            public Int32 prop1Cond;
            public Int32 prop2Cond;
            public Int32 prop3Cond;
            public Int32 prop4Cond;
            public Int32 prop5Cond;
            public Int32 prop6Cond;
            public Int32 property1;
            public Int32 property2;
            public Int32 property3;
            public Int32 property4;
            public Int32 property5;
            public Int32 property6;
            public Int32 onlyOnItemsRequiringUnitType;
        }

        public Affixes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AffixesTable>(data, ref offset, Count);
        }
    }
}
