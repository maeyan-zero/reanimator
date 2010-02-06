using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Quest : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class QuestTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string name;

            public Int32 code;
            public Int32 style;
            public Int32 nameStringKey;//idx
            public Int32 subscriberOnly;//bool
            public Int32 multiplayerOnly;//bool
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
            public Int32 offerReward;//idx
            public Int32 castGiver;//idx
            public Int32 levelStoryQuestStartsIn;//idx
            public Int32 castRewarder;//idx
            public Int32 levelRewarderNpc;//idx
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
            public string CompleteFunction;
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
            public Int32 removeOnJoinGame;
            public Int32 beatGameOnComplete;
            public Int32 undefined7;
            public Int32 weight;
            public float radius;
            public float height;
            public float flatZTolerance;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public Int32[] allowedDRLGStyles;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] levelDestinations;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string layoutAdventure;
            public Int32 objectAdventure;//idx
            public Int32 treasureRoomClass;//idx
            public Int32 template;//idx
            public Int32 descriptionDialog;//idx
            public Int32 inCompleteDialog;//idx
            public Int32 completeDialog;//idx
            public Int32 rewardDialog;//idx
            public Int32 unavailableDialog;//idx
            public Int32 acceptButtonText;//stridx
            public float timeLimit;
            public Int32 objectiveMonster;//idx
            public Int32 objectiveUnitType;//idx
            public Int32 objectiveObject;//idx
            public Int32 disableSpawning;//bool
            public Int32 objectiveCount;
            public float collectDropRate;
            public Int32 collectItem;//idx
            public float explorePercent;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string spawnNodeLabel;
            public Int32 spawnCount;
            public Int32 nameOverrideStringKey;//stridx
            public Int32 nameInLogOverrideStringKey;//stridx
            public Int32 logOverrideState;//idx
            public Int32 logOverrideString;//stridx
            public Int32 subLevelTypeTruthOld;
            public Int32 subLevelTypeTruthNew;
            public Int32 questStateAdvanceToAtSubLevelTrut;//idx
            public Int32 monsterBoss;//idx
            public Int32 globalthemeRequired;//idx
            public Int32 undefined8;
        }

        public Quest(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<QuestTable>(data, ref offset, Count);
        }
    }
}
