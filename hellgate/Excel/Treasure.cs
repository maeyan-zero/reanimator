using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Treasure
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string treasureClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 allowUnitTypes6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;
        public Int32 unknown02;
        public Int32 pickTypes;
        public Int32 picks;
        public Int32 undefined01;
        public Int32 undefined02;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] unknown03;
        public Int32 undefined03;//this needed to be changed so that the mutant dyes could drop randomly.
        public Int32 undefined04;
        public Int32 undefined05;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] unknown04;
        public float noDrop;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 levelBoost;
        public float moneyChanceMultiplier;
        public float moneyLuckChanceMultiplier;
        public float moneyAmountMultiplier;
        public Int32 item1m;/*this isn't actually defined, but it determines the type of object in item1, whether it's a specific item(01), a unit type(02),
            another treasure class(03), an item quality(04), or something else yet to be determined.*/
        public Int32 item1;
        public Int32 unknown05a;
        public Int32 unknown05b;
        public Int32 unknown05c;
        public Int32 unknown05d;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] unknown05efghi;
        public Int32 unknown05j;
        public Int32 unknown05k;
        public Int32 unknown05l;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] unknown05mnopqrs;
        public Int32 value1;
        public Int32 item2m;
        public Int32 item2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown06abcdefghi;
        public Int32 unknown06j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown06klmnopqrs;
        public Int32 value2;
        public Int32 item3m;
        public Int32 item3;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown07abcdefghi;
        public Int32 unknown07j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown07klmnopqrs;
        public Int32 value3;
        public Int32 item4m;
        public Int32 item4;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown08abcdefghi;
        public Int32 unknown08j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown08klmnopqrs;
        public Int32 value4;
        public Int32 item5m;
        public Int32 item5;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown09abcdefghi;
        public Int32 unknown09j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown09klmnopqrs;
        public Int32 value5;
        public Int32 item6m;
        public Int32 item6;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown10abcdefghi;
        public Int32 unknown10j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown10klmnopqrs;
        public Int32 value6;
        public Int32 item7m;
        public Int32 item7;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown11abcdefghi;
        public Int32 unknown11j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown11klmnopqrs;
        public Int32 value7;
        public Int32 item8m;
        public Int32 item8;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown12abcdefghi;
        public Int32 unknown12j;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] unknown12klmnopqrs;
        public Int32 value8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
        byte[] unknown13;
        [ExcelOutput(IsBitmask = true)]
        public Bitmask01 spawnCondition;
        public Int32 spawnFromMonsterUnitType;
        public Int32 spawnFromLevelTheme;

        [FlagsAttribute]
        public enum Bitmask01 : uint
        {
            createForAllPlayersInLevel = (1 << 0),
            requiredUsableByOperator = (1 << 1),
            requiredUsableBySpawner = (1 << 2),
            subscriberOnly = (1 << 3),
            maxSlots = (1 << 4),
            resultsNotRequired = (1 << 5),
            stackTreasure = (1 << 6),
            multiplayerOnly = (1 << 7),
            singlePlayerOnly = (1 << 8)
        }
    }
}