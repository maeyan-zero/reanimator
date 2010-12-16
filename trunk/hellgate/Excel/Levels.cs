using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Levels
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string levelName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        byte[] reserved;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 bitfieldIndex;
        public Int32 unknown1;
        public Int32 defaultSubLevel;
        public Int32 previousLevel;
        public Int32 nextLevel;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Level")]
        public Int32 levelDisplayName;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Level")]
        public Int32 floorSuffixName;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Level")]
        public Int32 finalSuffixFloorName;
        [ExcelOutput(IsBool = true)]
        public Int32 town;
        [ExcelOutput(IsBool = true)]
        public Int32 alwaysActive;
        [ExcelOutput(IsBool = true)]
        public Int32 startingLocation;
        public Int32 unknown2;
        public Int32 levelRestartRedirect;//index
        [ExcelOutput(IsBool = true)]
        public Int32 portalAndRecallLoc;
        [ExcelOutput(IsBool = true)]
        public Int32 firstPortalandRecallLoc;
        [ExcelOutput(IsBool = true)]
        public Int32 safe;
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
        public Int32 hellriftSubLevel1;//index
        public Int32 madLibs;//index
        public Int32 properNames;//index
        public Int32 adjectives;//index
        public Int32 nouns;//index
        public Int32 affixs;//index
        public Int32 suffixs;//index
        public Int32 subLevel1;//index
        public Int32 subLevel2;//index
        public Int32 subLevel3;//index
        public Int32 subLevel4;//index
        public Int32 subLevel5;//index
        public Int32 subLevel6;//index
        public Int32 subLevel7;//index
        public Int32 subLevel8;//index
        [ExcelOutput(IsBool = true)]
        public Int32 autoWaypoint;
        [ExcelOutput(IsBool = true)]
        public Int32 multiplayerOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 useTeamColors;
        public Int32 waypoint;//index
        public Int32 srvLevelType;
        public Int32 playerMax;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 monsterLevel;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 monsterLevelNightmare;
        public Int32 unknown3;
        [ExcelOutput(IsBool = true)]
        public Int32 monsterLevelFromParentLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 monsterLevelFromActivator;
        public Int32 monsterLevelActivatorDelta;
        public Int32 selectRandomThemePct;
        public Int32 partySizeRecommended;
        public Int32 questSpawnClass;
        public Int32 interactableSpawnClass;
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Level")]
        public Int32 stringEnter;//stridx
        [ExcelOutput(IsStringIndex = true, TableStringId = "Strings_Level")]
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
        public Int32 bonusStartingTreasure;//idx
        public Int32 sequenceNumber;
        public Int32 blank;//?idx
        public Int32 worldMapRow;
        public Int32 worldMapCol;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string worldMapFrame;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string worldMapFrameUnexplored;
        public Int32 worldMapColor;
        public Int32 worldMapLabelPos;
        public float worldMapLabelXOffs;
        public float worldMapLabelYOffs;
        public Int32 worldMapConnectIDs1;
        public Int32 worldMapConnectIDs2;
        public Int32 worldMapConnectIDs3;
        public Int32 worldMapConnectIDs4;
        public Int32 worldMapConnectIDs5;
        public Int32 worldMapConnectIDs6;
        public Int32 worldMapConnectIDs7;
        public Int32 worldMapConnectIDs8;
        public Int32 adventureChancePercent;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 numAdventures;
        [ExcelOutput(IsBool = true)]
        public Int32 enableRoomReset;
        public Int32 roomResetDelayInSeconds;
        public Int32 roomResetWhenMonstersBelowPercent;
        public float monsterRoomDensityMultiplier;
        [ExcelOutput(IsBool = true)]
        public Int32 contentsAlwaysRevealed;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 scriptPlayerEnterLevel;
        [ExcelOutput(IsBool = true)]
        public Int32 allowOverworldTravel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        byte[] unknown4;
    }
}