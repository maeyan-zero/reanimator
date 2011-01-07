using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsDrlgs
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string drlgRuleSet;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 drlgDisplayName;//stridx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 folderCode;
        [ExcelOutput(IsSecondaryString = true, SortColumnOrder = 2)]
        public Int32 style;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme11;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme13;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme14;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme15;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme16;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme17;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme18;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme19;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER_SETS")]
        public Int32 weatherSet;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_ENVIRONMENTS")]
        public Int32 environment;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 populateAllVisible;//bool
        public float overrideChampionChance;
        public float championZoneRadius;
        public float markerSpawnChance;
        public float monsterRoomDensity;
        public Int32 roomMonsterChance;
        public Int32 critterRoomDensity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 critterSpawnClass;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined2;
        public float maxTreasureDropRadius;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 randomState;//idx
        public Int32 randomStateRateMin;
        public Int32 randomStateRateMax;
        public Int32 randomStateRateDuration;
        [ExcelOutput(IsBool = true)]
        public Int32 havokFx;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 forceDrawAllRooms;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 isOutDoors;//bool
    }
}
