using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Levels : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string levelName;

            // is always zeros - assuming it's reserved, or a buffer or something.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            byte[] reserved;

            public Int32 code;
            public Int32 bitfieldIndex;
            public Int32 unknown1;
            public Int32 defaultSubLevel;
            public Int32 previousLevel;
            public Int32 nextLevel;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Level")]
            public Int32 levelDisplayName;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Level")]
            public Int32 floorSuffixName;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Level")]
            public Int32 finalSuffixFloorName;
            public Int32 town;//bool
            public Int32 alwaysActive;//bool
            public Int32 startingLocation;//bool
            public Int32 unknown2;
            public Int32 levelRestartRedirect;//index
            public Int32 portalAndRecallLoc;//bool
            public Int32 firstPortalandRecallLoc;//bool
            public Int32 safe;//bool
            public Int32 subLevelTownPortal;//idx
            public Int32 tutorial;//bool
            public Int32 disableTownPortals;//bool
            public Int32 onEnterClearBadStates;//bool
            public Int32 playerOwnedCannotDie;//bool
            public Int32 hardcoreDeadCanVisit;//bool
            public Int32 canFormAutoParties;//bool
            public float CameraDollyScale;
            public float minZ;
            public float maxZ;
            public float visibilityOpacity;
            public float fixedOrientation;
            public Int32 orientedToMap;//bool
            public Int32 allowRandomOrientation;//bool
            public Int32 hellriftChancePercent;
            public Int32 hellriftSubLevel;//index
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
            public Int32 autoWaypoint;//bool
            public Int32 multiplayerOnly;//bool
            public Int32 useTeamColors;//bool
            public Int32 waypoint;//index
            public Int32 srvLevelType;
            public Int32 playerMax;
            public Int32 monsterLevel;//intptr
            public Int32 monsterLevelNightmare;//intptr
            public Int32 unknown3;
            public Int32 monsterLevelFromParentLevel;//bool
            public Int32 monsterLevelFromActivator;//bool
            public Int32 monsterLevelActivatorDelta;
            public Int32 selectRandomThemePct;
            public Int32 partySizeRecommended;
            public Int32 questSpawnClass;
            public Int32 interactableSpawnClass;
            public Int32 stringEnter;//stridx
            public Int32 stringLeave;//stridx
            public float championSpawnChancePercentAtEachSpawnL;//L == Level?
            public float uniqueMonsterChancePercent;
            public Int32 mapX;
            public Int32 mapY;
            public Int32 firstLevel;//bool
            public Int32 firstLevelCheating;//bool
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
            public Int32 numAdventures;//intptr
            public Int32 enableRoomReset;//bool
            public Int32 roomResetDelayInSeconds;
            public Int32 roomResetWhenMonstersBelowPercent;
            public float monsterRoomDensityMultiplier;
            public Int32 contentsAlwaysRevealed;//bool
            public Int32 scriptPlayerEnterLevel;//intptr
            public Int32 allowOverworldTravel;//bool
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            byte[] unknown4;
        }

        public Levels(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsTable>(data, ref offset, Count);
        }
    }
}
