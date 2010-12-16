using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Quest
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 style;
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
        public Int32 autoActivateLevel;//idx
        public Int32 onlyOneDifficulty;
        public Int32 questPreReqs01;
        public Int32 questPreReqs02;
        public Int32 questPreReqs03;
        public Int32 questPreReqs04;
        public Int32 questPreReqs05;
        public Int32 questPreReqs06;
        public Int32 questPreReqs07;
        public Int32 questPreReqs08;
        public Int32 currentlyUnavailable;
        public Int32 minLevelPreReq;
        public Int32 maxLevelPreReq;
        public Int32 factionTypePreReq;//idx
        public Int32 factionAmountPreReq;
        public Int32 levelsPreLoadedWith01;
        public Int32 levelsPreLoadedWith02;
        public Int32 levelsPreLoadedWith03;
        public Int32 levelsPreLoadedWith04;
        public Int32 levelsPreLoadedWith05;
        public Int32 levelsPreLoadedWith06;
        public Int32 levelsPreLoadedWith07;
        public Int32 levelsPreLoadedWith08;
        public Int32 levelsPreLoadedWith09;
        public Int32 levelsPreLoadedWith10;
        public Int32 levelsPreLoadedWith11;
        public Int32 levelsPreLoadedWith12;
        public Int32 levelsPreLoadedWith13;
        public Int32 levelsPreLoadedWith14;
        public Int32 levelsPreLoadedWith15;
        public Int32 levelsPreLoadedWith16;
        public Int32 startingItemsTreasureClass;//idx
        public Int32 removeStartingItemsOnComplete;
        [ExcelOutput(SortColumnOrder = 3)]
        public Int32 offerReward;//idx
        public Int32 castGiver;//idx
        public Int32 levelStoryQuestStartsIn;//idx
        public Int32 castRewarder;//idx
        public Int32 levelRewarderNpc;//idx
        [ExcelOutput(SortColumnOrder = 4)]
        public Int32 giverItem;//idx
        public Int32 giverItemMonster;//idx
        public Int32 giverItemLevel;//idx
        public float giverItemDropRate;
        public Int32 level;
        public Int32 levelNightmare;
        public Int32 undefined1;
        public float experienceMultiplier;
        public float moneyMultiplier;
        public Int32 statPoints;
        public Int32 skillPoints;
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
        public Int32 warpToOpenOnActivate;//idx
        public Int32 warpToOpenOnComplete;//idx
        public Int32 itemsToRemoveOnAbandon01;
        public Int32 itemsToRemoveOnAbandon02;
        public Int32 itemsToRemoveOnAbandon03;
        public Int32 itemsToRemoveOnAbandon04;
        public Int32 itemsToRemoveOnAbandon05;
        public Int32 itemsToRemoveOnAbandon06;
        public Int32 itemsToRemoveOnAbandon07;
        public Int32 itemsToRemoveOnAbandon08;
        public Int32 itemsToRemoveOnAbandon09;
        public Int32 itemsToRemoveOnAbandon10;
        public Int32 itemsToRemoveOnAbandon11;
        public Int32 itemsToRemoveOnAbandon12;
        public Int32 itemsToRemoveOnAbandon13;
        public Int32 itemsToRemoveOnAbandon14;
        public Int32 itemsToRemoveOnAbandon15;
        public Int32 itemsToRemoveOnAbandon16;
        public Int32 itemsToRemoveOnAbandon17;
        public Int32 itemsToRemoveOnAbandon18;
        public Int32 itemsToRemoveOnAbandon19;
        public Int32 itemsToRemoveOnAbandon20;
        public Int32 itemsToRemoveOnAbandon21;
        public Int32 itemsToRemoveOnAbandon22;
        public Int32 itemsToRemoveOnAbandon23;
        public Int32 itemsToRemoveOnAbandon24;
        public Int32 itemsToRemoveOnAbandon25;
        public Int32 itemsToRemoveOnAbandon26;
        public Int32 itemsToRemoveOnAbandon27;
        public Int32 itemsToRemoveOnAbandon28;
        public Int32 itemsToRemoveOnAbandon29;
        public Int32 itemsToRemoveOnAbandon30;
        public Int32 itemsToRemoveOnAbandon31;
        public Int32 itemsToRemoveOnAbandon32;
        public Int32 itemsToRemoveOnComplete01;
        public Int32 itemsToRemoveOnComplete02;
        public Int32 itemsToRemoveOnComplete03;
        public Int32 itemsToRemoveOnComplete04;
        public Int32 itemsToRemoveOnComplete05;
        public Int32 itemsToRemoveOnComplete06;
        public Int32 itemsToRemoveOnComplete07;
        public Int32 itemsToRemoveOnComplete08;
        public Int32 itemsToRemoveOnComplete09;
        public Int32 itemsToRemoveOnComplete10;
        public Int32 itemsToRemoveOnComplete11;
        public Int32 itemsToRemoveOnComplete12;
        public Int32 itemsToRemoveOnComplete13;
        public Int32 itemsToRemoveOnComplete14;
        public Int32 itemsToRemoveOnComplete15;
        public Int32 itemsToRemoveOnComplete16;
        public Int32 itemsToRemoveOnComplete17;
        public Int32 itemsToRemoveOnComplete18;
        public Int32 itemsToRemoveOnComplete19;
        public Int32 itemsToRemoveOnComplete20;
        public Int32 itemsToRemoveOnComplete21;
        public Int32 itemsToRemoveOnComplete22;
        public Int32 itemsToRemoveOnComplete23;
        public Int32 itemsToRemoveOnComplete24;
        public Int32 itemsToRemoveOnComplete25;
        public Int32 itemsToRemoveOnComplete26;
        public Int32 itemsToRemoveOnComplete27;
        public Int32 itemsToRemoveOnComplete28;
        public Int32 itemsToRemoveOnComplete29;
        public Int32 itemsToRemoveOnComplete30;
        public Int32 itemsToRemoveOnComplete31;
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
        public Int32 allowedDRLGStyles02;
        public Int32 allowedDRLGStyles03;
        public Int32 allowedDRLGStyles04;
        public Int32 allowedDRLGStyles05;
        public Int32 allowedDRLGStyles06;
        public Int32 allowedDRLGStyles07;
        public Int32 allowedDRLGStyles08;
        public Int32 allowedDRLGStyles09;
        public Int32 allowedDRLGStyles10;
        public Int32 allowedDRLGStyles11;
        public Int32 allowedDRLGStyles12;
        public Int32 allowedDRLGStyles13;
        public Int32 allowedDRLGStyles14;
        public Int32 allowedDRLGStyles15;
        public Int32 allowedDRLGStyles16;
        public Int32 levelDestinations01;
        public Int32 levelDestinations02;
        public Int32 levelDestinations03;
        public Int32 levelDestinations04;
        public Int32 levelDestinations05;
        public Int32 levelDestinations06;
        public Int32 levelDestinations07;
        public Int32 levelDestinations08;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string layoutAdventure;
        public Int32 objectAdventure;//idx
        public Int32 treasureRoomClass;//idx
        public Int32 template;//idx
        //[ExcelOutput(IsStringId = true, TableStringId = "Strings_Strings")]
        public Int32 descriptionDialog;//idx
        //[ExcelOutput(IsStringId = true, TableStringId = "Strings_Strings")]
        public Int32 inCompleteDialog;//idx
        //[ExcelOutput(IsStringId = true, TableStringId = "Strings_Strings")]
        public Int32 completeDialog;//idx
        //[ExcelOutput(IsStringId = true, TableStringId = "Strings_Strings")]
        public Int32 rewardDialog;//idx
        public Int32 unavailableDialog;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 acceptButtonText;//stridx
        public float timeLimit;
        public Int32 objectiveMonster;//idx
        public Int32 objectiveUnitType;//idx
        public Int32 objectiveObject;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 disableSpawning;//bool
        public Int32 objectiveCount;
        public float collectDropRate;
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
        public Int32 subLevelTypeTruthOld;
        public Int32 subLevelTypeTruthNew;
        public Int32 questStateAdvanceToAtSubLevelTrut;//idx
        public Int32 monsterBoss;//idx
        public Int32 globalthemeRequired;//idx
        Int32 undefined8;
    }
}