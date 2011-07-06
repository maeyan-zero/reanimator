using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelDrlgsRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String DrlgName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String DrlgRuleSet;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 DrlgDisplayName;//stridx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 FolderCode;
        [ExcelOutput(IsSecondaryString = true, SortColumnOrder = 2)]
        public Int32 Style;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public Int32[] Themes;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER_SETS")]
        public Int32 WeatherSet;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_ENVIRONMENTS")]
        public Int32 Environment;//idx
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
		public Int32 overrideFolderCodeMask1;
		public Int32 overrideFolderCodeMask2;
		public Int32 overrideFolderCodeMask3;
		public Int32 overrideFolderCodeMask4;
		public Int32 overrideFolderCodeMask5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        Int32[] undefined3;
    }
}
