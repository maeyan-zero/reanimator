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
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        public Int32 style;
        [ExcelOutput(IsStringID = true)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        Int32[] levelsPreLoadedWith;
        public Int32 startingItemsTreasureClass;//idx
        public Int32 removeStartingItemsOnComplete;
        [ExcelOutput(SortAscendingID = 3, RequiresDefault = true)]
        public Int32 offerReward;//idx
        public Int32 castGiver;//idx
        public Int32 levelStoryQuestStartsIn;//idx
        public Int32 castRewarder;//idx
        public Int32 levelRewarderNpc;//idx
        [ExcelOutput(SortAscendingID = 4, RequiresDefault = true)]
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        Int32[] itemsToRemoveOnAbandon;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        Int32[] itemsToRemoveOnComplete;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] levelDestinations;
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
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Strings")]
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
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Names")]
        public Int32 nameOverrideStringKey;
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Names")]
        public Int32 nameInLogOverrideStringKey;
        public Int32 logOverrideState;
        [ExcelOutput(IsStringID = true, TableStringID = "Strings_Quest")]
        public Int32 logOverrideString;
        public Int32 subLevelTypeTruthOld;
        public Int32 subLevelTypeTruthNew;
        public Int32 questStateAdvanceToAtSubLevelTrut;//idx
        public Int32 monsterBoss;//idx
        public Int32 globalthemeRequired;//idx
        Int32 undefined8;
    }
}
