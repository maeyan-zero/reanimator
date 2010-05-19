using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class MonsterQuality : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MonsterQualityTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string quality;

            public Int32 code;
            public Int32 rarity;
            public Int32 nameColor;//idx
            public Int32 displayNameStringKey;//stridx
            public Int32 championFormatStringKey;//stridx
            public Int32 pickProperName;//bool
            public Int32 type;
            public Int32 undefined1;
            public byte AppearanceHeightMin;
            public byte AppearanceHeightMax;
            public byte AppearanceWeightMin;
            public byte AppearanceWeightMax;
            //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32 undefined2;
            public Single undefined3;
            public Int32 State;//idx
            public float MoneyChanceMultiplier;
            public float MoneyAmountMultiplier;
            public Int32 TreasureLevelBoost;
            public float HealthMultiplier;
            public float ToHitMultiplier;
            public Int32 MinionPackSize;//intptr
            public Int32 prop1;//intptr
            public Int32 prop2;//intptr
            public Int32 prop3;//intptr
            public Int32 AffixCount;
            public Int32 AffixType1;//idx
            public Int32 AffixType2;//idx
            public Int32 AffixType3;//idx
            public Int32 AffixProbability1;//intptr
            public Int32 AffixProbability2;//intptr
            public Int32 AffixProbability3;//intptr
            public Int32 experienceMultiplier;
            public Int32 luckMultiplier;
            public Int32 MonsterQualityDowngrade;//idx
            public Int32 MinionQuality;//idx
            public Int32 ShowLabel;//bool
            public Int32 TreasureClass;//idx
        }

        public MonsterQuality(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MonsterQualityTable>(data, ref offset, Count);
        }
    }
}
