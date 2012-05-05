using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TreasureTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string treasureClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public Int32[] allowUnitTypes;
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;
        Int32 unknown02;
        public PickTypes pickType;  // XLS_ReadInternalIndex_PickType
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Int32[] picks;
        public float noDrop;
        public Int32 mustDrop_tcv4;
        [ExcelOutput(IsScript = true)]
        public Int32 levelBoost;
        public float gamblePriceRangeMin_tcv4;
        public float gamblePriceRangeMax_tcv4;
        public float moneyChanceMultiplier;
        public float moneyLuckChanceMultiplier;
        public float moneyAmountMultiplier;

        /* first index is type, which determines whether next index is a specific item(01), a unit type(02),
         * another treasure class(03), an item quality(04), or something else yet to be determined */

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item1; // is 0x58 (22x Int32; 21x item, 1x value) in length - is multiple relational index like SPAWN_CLASS
        public Int32 value1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item2;
        public Int32 value2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item3;
        public Int32 value3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item4;
        public Int32 value4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item5;
        public Int32 value5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item6;
        public Int32 value6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item7;
        public Int32 value7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public Int32[] item8;
        public Int32 value8;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
        byte[] unknown13;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask01 spawnCondition;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 spawnFromMonsterUnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 spawnFromLevelTheme;

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            CreateForAllPlayersInLevel = (1 << 0),
            RequiredUsableByOperator = (1 << 1),
            RequiredUsableBySpawner = (1 << 2),
            SubscriberOnly = (1 << 3),
            MaxSlots = (1 << 4),
            ResultsNotRequired = (1 << 5),
            StackTreasure = (1 << 6),
            MultiplayerOnly = (1 << 7),
            Undefined1 = (1 << 8),
            BaseOnPlayerLevelTcv4 = (1 << 9),
            RecipientRequiresStateTcv4 = (1 << 10),
            Undefined2 = (1 << 11),
            SinglePlayerOnly = (1 << 12)
        }

        public enum PickTypes
        {
            Null = -1,
            one = 0,
            all = 1,
            modifiers_only = 2,
            ind_percent = 3,
            one_eliminate = 4,
            first_valid = 5
        }
    }
}