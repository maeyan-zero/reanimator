using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Levels
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string levelName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        byte[] reserved;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 bitfieldIndex;
        public Int32 unknown1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 defaultSubLevel;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 previousLevel;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 nextLevel;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 levelDisplayName;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 floorSuffixName;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 finalSuffixFloorName;
        [ExcelOutput(IsBool = true)]
        public Int32 town;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysActive;
        [ExcelOutput(IsBool = true)]
        public Int32 startingLocation;
        public Int32 unknown2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelRestartRedirect;//index
        [ExcelOutput(IsBool = true)]
        public Int32 portalAndRecallLoc;
        [ExcelOutput(IsBool = true)]
        public Int32 firstPortalandRecallLoc;
        [ExcelOutput(IsBool = true)]
        public Int32 safe;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevelTownPortal;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 tutorial;
        [ExcelOutput(IsBool = true)]
        public Int32 disableTownPortals;
        [ExcelOutput(IsBool = true)]
        public Int32 onEnterClearBadStates;
        [ExcelOutput(IsBool = true)]
        public Int32 playerOwnedCannotDie;
        [ExcelOutput(IsBool = true)]
        public Int32 hardcoreDeadCanVisit;
        [ExcelOutput(IsBool = true)]
        public Int32 canFormAutoParties;
        public float CameraDollyScale;
        public float minZ;
        public float maxZ;
        public float visibilityOpacity;
        public float fixedOrientation;
        [ExcelOutput(IsBool = true)]
        public Int32 orientedToMap;
        [ExcelOutput(IsBool = true)]
        public Int32 allowRandomOrientation;
        public Int32 hellriftChancePercent;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 hellriftSubLevel1;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 madLibs;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 properNames;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 adjectives;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 nouns;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 affixs;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "EXCELTABLES")]
        public Int32 suffixs;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel1;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel2;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel3;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel4;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel5;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel6;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel7;//index
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevel8;//index
        [ExcelOutput(IsBool = true)]
        public Int32 autoWaypoint;
        [ExcelOutput(IsBool = true)]
        public Int32 multiplayerOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 useTeamColors;
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 waypoint;//index
        public Int32 srvLevelType; // XLS_InternalIndex_SrvLevelType (XLS_LEVEL_DEFINITION+576), 0x07
        public Int32 playerMax;
        [ExcelOutput(IsScript = true)]
        public Int32 monsterLevel;
        [ExcelOutput(IsScript = true)]
        public Int32 monsterLevelNightmare;
        public Int32 unknown3;
        [ExcelOutput(IsBool = true)]
        public Int32 monsterLevelFromParentLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 monsterLevelFromActivator;
        public Int32 monsterLevelActivatorDelta;
        public Int32 selectRandomThemePct;
        public Int32 partySizeRecommended;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 questSpawnClass;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 interactableSpawnClass;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringEnter;//stridx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringLeave;//stridx
        public float championSpawnChancePercentAtEachSpawnLocation;
        public float uniqueMonsterChancePercent;
        public Int32 mapX;
        public Int32 mapY;
        [ExcelOutput(IsBool = true)]
        public Int32 firstLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 firstLevelCheating;
        public Int32 playerExpLevel;
        public Int32 startingGold;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 bonusStartingTreasure;//idx
        public Int32 sequenceNumber;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ACT")]
        public Int32 act;//?idx
        public Int32 worldMapRow;
        public Int32 worldMapCol;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string worldMapFrame;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string worldMapFrameUnexplored;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FONTCOLORS")]
        public Int32 worldMapColor;
        public Int32 worldMapLabelPos; // XLS_InternalIndex_WorldMapLabelPos (XLS_LEVEL_DEFINITION+A61), 0x0C
        public float worldMapLabelXOffs;
        public float worldMapLabelYOffs;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 worldMapConnectIDs8;
        public Int32 adventureChancePercent;
        [ExcelOutput(IsScript = true)]
        public Int32 numAdventures;
        [ExcelOutput(IsBool = true)]
        public Int32 enableRoomReset;
        public Int32 roomResetDelayInSeconds;
        public Int32 roomResetWhenMonstersBelowPercent;
        public float monsterRoomDensityMultiplier;
        [ExcelOutput(IsBool = true)]
        public Int32 contentsAlwaysRevealed;
        [ExcelOutput(IsScript = true)]
        public Int32 scriptPlayerEnterLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 allowOverworldTravel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] unknown4;
        Int32 unknown5;
    }
}