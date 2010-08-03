using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(SortId = 2)]
        public Int32 code;
        public Int32 style;
        [ExcelOutput(IsStringId = true, Table = "Strings_Quest")]
        public Int32 nameStringKey;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 subscriberOnly;//bool
        [ExcelOutput(IsBool = true)]
        Int32 multiplayerOnly;//bool
        public Int32 startingQuestCheat;
        Int32 questCheatCompleted;
        public Int32 closeOnComplete;
        public Int32 repeatable;
        Int32 hideQuestLog;
        public Int32 repeatRateInSeconds;
        public Int32 skipActivateFanfare;
        public Int32 skipCompleteFanfare;
        public Int32 endOfActNumber;
        public Int32 autoTrackOnActivate;
        public Int32 autoActivateLevel;//idx
        public Int32 onlyOneDifficulty;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] questPreReqs;
        public Int32 currentlyUnavailable;
        public Int32 minLevelPreReq;
        public Int32 maxLevelPreReq;
        public Int32 factionTypePreReq;//idx
        public Int32 factionAmountPreReq;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Int32[] levelsPreLoadedWith;
        public Int32 startingItemsTreasureClass;//idx
        public Int32 removeStartingItemsOnComplete;
        [ExcelOutput(SortId = 3)]
        public Int32 offerReward;//idx
        public Int32 castGiver;//idx
        public Int32 levelStoryQuestStartsIn;//idx
        public Int32 castRewarder;//idx
        public Int32 levelRewarderNpc;//idx
        [ExcelOutput(SortId = 4)]
        public Int32 giverItem;//idx
        Int32 giverItemMonster;//idx
        Int32 giverItemLevel;//idx
        public float giverItemDropRate;
        public Int32 level;
        public Int32 levelNightmare;
        Int32 undefined1;
        public float experienceMultiplier;
        public float moneyMultiplier;
        public Int32 statPoints;
        public Int32 skillPoints;
        public Int32 factionTypeReward;//idx
        public Int32 factionAmountReward;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InitFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string FreeFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string onEnterGameFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        string CompleteFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined5;
        public Int32 warpToOpenOnActivate;//idx
        public Int32 warpToOpenOnComplete;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Int32[] itemsToRemoveOnAbandon;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Int32[] itemsToRemoveOnComplete;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string versionFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefined6;
        Int32 removeOnJoinGame;
        public Int32 beatGameOnComplete;
        Int32 undefined7;
        public Int32 weight;
        public float radius;
        public float height;
        public float flatZTolerance;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Int32[] allowedDRLGStyles;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] levelDestinations;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        string layoutAdventure;
        public Int32 objectAdventure;//idx
        public Int32 treasureRoomClass;//idx
        public Int32 template;//idx
        //[ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 descriptionDialog;//idx
        //[ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 inCompleteDialog;//idx
        //[ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 completeDialog;//idx
        //[ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 rewardDialog;//idx
        Int32 unavailableDialog;//idx
        [ExcelOutput(IsStringId = true, Table = "Strings_Strings")]
        public Int32 acceptButtonText;//stridx
        float timeLimit;
        public Int32 objectiveMonster;//idx
        public Int32 objectiveUnitType;//idx
        public Int32 objectiveObject;//idx
        [ExcelOutput(IsBool = true)]
        Int32 disableSpawning;//bool
        public Int32 objectiveCount;
        public float collectDropRate;
        public Int32 collectItem;//idx
        public float explorePercent;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        string spawnNodeLabel;
        public Int32 spawnCount;
        [ExcelOutput(IsStringId = true, Table = "Strings_Names")]
        public Int32 nameOverrideStringKey;//stridx
        [ExcelOutput(IsStringId = true, Table = "Strings_Names")]
        public Int32 nameInLogOverrideStringKey;//stridx
        public Int32 logOverrideState;//idx
        [ExcelOutput(IsStringId = true, Table = "Strings_Quest")]
        public Int32 logOverrideString;//stridx
        public Int32 subLevelTypeTruthOld;
        public Int32 subLevelTypeTruthNew;
        public Int32 questStateAdvanceToAtSubLevelTrut;//idx
        public Int32 monsterBoss;//idx
        public Int32 globalthemeRequired;//idx
        Int32 undefined8;
    }
}