using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterQualityRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string quality;

        public Int32 code;
        public Int32 rarity;
        public Int32 nameColor;//idx
        [ExcelFile.ExcelOutputAttribute(IsStringId = true, Table = "Strings_Strings")]
        public Int32 displayNameStringKey;//stridx
        [ExcelFile.ExcelOutputAttribute(IsStringId = true, Table = "Strings_Affix")]
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
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.PropData))]
        public Int32 prop1;//intptr
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.PropData))]
        public Int32 prop2;//intptr
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.PropData))]
        public Int32 prop3;//intptr
        public Int32 AffixCount;
        public Int32 AffixType1;//idx
        public Int32 AffixType2;//idx
        public Int32 AffixType3;//idx
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.SingleUInt32))]
        public Int32 AffixProbability1;//intptr
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.SingleUInt32))]
        public Int32 AffixProbability2;//intptr
        [ExcelFile.ExcelOutputAttribute(IsIntOffset = true, IntOffsetType = typeof(ExcelFile.SingleUInt32))]
        public Int32 AffixProbability3;//intptr
        public Int32 experienceMultiplier;
        public Int32 luckMultiplier;
        public Int32 MonsterQualityDowngrade;//idx
        public Int32 MinionQuality;//idx
        [ExcelFile.ExcelOutputAttribute(IsBool = true)]
        public Int32 ShowLabel;//bool
        public Int32 TreasureClass;//idx
    }
}