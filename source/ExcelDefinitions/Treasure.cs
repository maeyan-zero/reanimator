using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TreasureRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown03;
        public Int32 undefined03;//this needed to be changed so that the mutant dyes could drop randomly.
        public Int32 undefined04;
        public Int32 undefined05;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        byte[] unknown04;
        public float noDrop;
        [ExcelOutput(IsIntOffset = true, IntOffsetOrder = 1)]
        public Int32 levelBoost;
        public float moneyChanceMultiplier;
        public float moneyLuckChanceMultiplier;
        public float moneyAmountMultiplier;
        public Int32 item1m;/*this isn't actually defined, but it determines the type of object in item1, whether it's a specific item(01), a unit type(02),
            another treasure class(03), an item quality(04), or something else yet to be determined.*/
        public Int32 item1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]   // this is equal to 19 Int32
        byte[] unknown05;
        public Int32 value1;
        public Int32 item2m;
        public Int32 item2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown06;
        public Int32 value2;
        public Int32 item3m;
        public Int32 item3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown07;
        public Int32 value3;
        public Int32 item4m;
        public Int32 item4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown08;
        public Int32 value4;
        public Int32 item5m;
        public Int32 item5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown09;
        public Int32 value5;
        public Int32 item6m;
        public Int32 item6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown10;
        public Int32 value6;
        public Int32 item7m;
        public Int32 item7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown11;
        public Int32 value7;
        public Int32 item8m;
        public Int32 item8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 76)]
        byte[] unknown12;
        public Int32 value8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8804)]
        byte[] unknown13;
        [ExcelOutput(IsBitmask = true)]
        public Treasure.Bitmask01 spawnCondition;
        public Int32 spawnFromMonsterUnitType;
        public Int32 spawnFromLevelTheme;

        public abstract class Treasure
        {
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
            };
        }
    }
}