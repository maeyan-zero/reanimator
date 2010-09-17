using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Treasure
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string treasureClass;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes1;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes2;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes3;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes4;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes5;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "UNITTYPES")]
        public UInt32 allowUnitTypes6;
        [ExcelAttribute(IsTableIndex = true, TableStringID = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;
        UInt32 null01;
        [ExcelAttribute(IsValueList = true)]
        public PickTypes pickTypes;
        public UInt32 picks;
        public UInt32 undefined01;
        public UInt32 undefined02;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        UInt32[] null02;
        public UInt32 undefined03;
        public UInt32 undefined04;
        public UInt32 undefined05;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        UInt32[] null03;
        public Single noDrop;
        [ExcelAttribute(IsIntOffset = true)]
        public UInt32 levelBoost;
        public Single moneyChanceMultiplier;
        public Single moneyLuckChanceMultiplier;
        public Single moneyAmountMultiplier;
        [ExcelAttribute(IsValueList = true)]
        public Table item01Table;
        public Int32 item01Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null04;
        public UInt32 item01Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item02Table;
        public Int32 item02Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null05;
        public UInt32 item02Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item03Table;
        public Int32 item03Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null06;
        public UInt32 item03Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item04Table;
        public Int32 item04Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null07;
        public UInt32 item04Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item05Table;
        public Int32 item05Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null08;
        public UInt32 item05Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item06Table;
        public Int32 item06Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null09;
        public UInt32 item06Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item07Table;
        public Int32 item07Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null10;
        public UInt32 item07Count;
        [ExcelAttribute(IsValueList = true)]
        public Table item08Table;
        public Int32 item08Index;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        UInt32[] null11;
        public UInt32 item08Count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2201)]
        UInt32[] null12;
        [ExcelAttribute(IsBitmask = true)]
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

        public enum PickTypes : int
        {
            nothing = -1,
            unknown00 = 0,
            unknown01 = 1,
            unknown02 = 2,
            unknown03 = 3,
            _null01 = 4,
            unknown05 = 5
        }

        public enum Table : uint
        {
            nothing = 0,
            item = 1,
            unitType = 2,
            treasure = 3,
            itemQuality = 4,
            unknown05 = 5
        }
    }
}