using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterQuality
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string quality;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 rarity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 nameColor;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayNameStringKey;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 championFormatStringKey;//stridx
        [ExcelOutput(IsBool = true)]
        public Int32 pickProperName;//bool
        public Type type;
        public Int32 undefined1;
        public byte AppearanceHeightMin;
        public byte AppearanceHeightMax;
        public byte AppearanceWeightMin;
        public byte AppearanceWeightMax;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Int32 undefined2;
        public Single undefined3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 State;//idx
        public float MoneyChanceMultiplier;
        public float MoneyAmountMultiplier;
        public Int32 TreasureLevelBoost;
        public float HealthMultiplier;
        public float ToHitMultiplier;
        [ExcelOutput(IsScript = true)]
        public Int32 MinionPackSize;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop1;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop2;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 prop3;//intptr
        public Int32 AffixCount;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 AffixType1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 AffixType2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIXTYPES")]
        public Int32 AffixType3;//idx
        [ExcelOutput(IsScript = true)]
        public Int32 AffixProbability1;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 AffixProbability2;//intptr
        [ExcelOutput(IsScript = true)]
        public Int32 AffixProbability3;//intptr
        public Int32 experienceMultiplier;
        public Int32 luckMultiplier;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTER_QUALITY")]
        public Int32 MonsterQualityDowngrade;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTER_QUALITY")]
        public Int32 MinionQuality;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 ShowLabel;//bool
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 TreasureClass;//idx

        public enum Type
        {
            Null = -1,
            None = 0,
            Champion = 1,
            TopChampion = 2,
            Unique = 3
        }

    }
}
