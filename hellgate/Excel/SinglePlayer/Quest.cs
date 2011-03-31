using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Quest
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Style style;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 nameStringKey;
        [ExcelOutput(IsBool = true)]
        public Int32 subscriberOnly;
        [ExcelOutput(IsBool = true)]
        public Int32 multiplayerOnly;
        public Int32 startingQuestCheat;
        public Int32 questCheatCompleted;
        public Int32 closeOnComplete;
        public Int32 repeatable;
        public Int32 hideQuestLog;
        public Int32 repeatRateInSeconds;
        public Int32 skipActivateFanfare;
        public Int32 skipCompleteFanfare;
        public Int32 endOfActNumber;
        public Int32 autoTrackOnActivate;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 autoActivateLevel;//idx
        public Int32 onlyOneDifficulty;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs02;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs03;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs04;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs05;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs06;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs07;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "QUEST")]
        public Int32 questPreReqs08;
        public Int32 currentlyUnavailable;
        public Int32 minLevelPreReq;
        public Int32 maxLevelPreReq;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION")]
        public Int32 factionTypePreReq;//idx
        public Int32 factionAmountPreReq;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith01;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith02;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith03;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith04;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith05;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith06;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith07;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith08;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith09;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith10;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith11;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith12;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelsPreLoadedWith16;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 startingItemsTreasureClass;//idx
        public Int32 removeStartingItemsOnComplete;
        [ExcelOutput(SortColumnOrder = 3, IsTableIndex = true, TableStringId = "OFFER")]
        public Int32 offerReward;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_CAST")]
        public Int32 castGiver;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelStoryQuestStartsIn;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_CAST")]
        public Int32 castRewarder;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelRewarderNpc;//idx
        [ExcelOutput(SortColumnOrder = 4, IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 giverItem;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 giverItemMonster;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 giverItemLevel;//idx
        public float giverItemDropRate;
        public Int32 level;
        public Int32 levelNightmare;
        public Int32 undefined1;
        public float experienceMultiplier;
        public float moneyMultiplier;
        public Int32 statPoints;
        public Int32 skillPoints;
        [ExcelOutput(IsTableIndex = true, TableStringId = "FACTION")]
        public Int32 factionTypeReward;//idx
        public Int32 factionAmountReward;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InitFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FreeFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string onEnterGameFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string CompleteFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 warpToOpenOnActivate;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 warpToOpenOnComplete;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon02;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon03;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon04;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon05;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon06;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon07;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon08;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon09;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon11;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon16;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon17;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon18;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon19;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon20;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon21;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon22;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon23;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon24;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon25;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon26;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon27;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon28;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon29;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon30;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon31;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnAbandon32;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete02;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete03;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete04;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete05;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete06;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete07;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete08;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete09;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete11;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete16;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete17;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete18;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete19;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete20;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete21;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete22;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete23;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete24;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete25;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete26;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete27;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete28;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete29;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete30;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete31;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 itemsToRemoveOnComplete32;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string versionFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined6;
        public Int32 removeOnJoinGame;
        public Int32 beatGameOnComplete;
        Int32 undefined7;
        public Int32 weight;
        public float radius;
        public float height;
        public float flatZTolerance;
        public Int32 allowedDRLGStyles01;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles02;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles03;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles04;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles05;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles06;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles07;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles08;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles09;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles10;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles11;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles12;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedDRLGStyles16;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations01;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations02;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations03;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations04;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations05;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations06;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations07;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelDestinations08;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string layoutAdventure;
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 objectAdventure;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasureRoomClass;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 template;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 descriptionDialog;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 inCompleteDialog;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 completeDialog;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 rewardDialog;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIALOG")]
        public Int32 unavailableDialog;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 acceptButtonText;//stridx
        public float timeLimit;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 objectiveMonster;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 objectiveUnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "OBJECTS")]
        public Int32 objectiveObject;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 disableSpawning;//bool
        public Int32 objectiveCount;
        public float collectDropRate;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 collectItem;//idx
        public float explorePercent;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string spawnNodeLabel;
        public Int32 spawnCount;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 nameOverrideStringKey;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 nameInLogOverrideStringKey;
        public Int32 logOverrideState;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 logOverrideString;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevelTypeTruthOld;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SUBLEVEL")]
        public Int32 subLevelTypeTruthNew;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUEST_STATE")]
        public Int32 questStateAdvanceToAtSubLevelTruth;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monsterBoss;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalthemeRequired;//idx
        public Int32 undefined8;

        public enum Style : uint
        {
            StoryLine,
            Optional,
            _2,
            Truth,
            Adventure
        }
    }
}