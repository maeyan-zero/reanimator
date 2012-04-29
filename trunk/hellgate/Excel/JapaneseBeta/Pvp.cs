using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 code;
        public Int32 respawnDelayInSec;
        public Int32 countdownDuration;
        public Int32 defaultNumberOfrounds;
        public Int32 defaultRoundDuration;
        public float winContributionBalanceValue;
        public float winContributionBalanceDamageDone;
        public float winContributionBalanceDamageTaken;
        public float winContributionBalanceNumOfKills;
        public float winContributionBalanceNumOfDeaths;
        public float winContributionBalanceObjective1;
        public float winContributionBalanceObjective2;
        public float winContributionBalanceObjective3;
        public float lossContributionBalanceValue;
        public float lossContributionBalanceDamageDone;
        public float lossContributionBalanceDamageTaken;
        public float lossContributionBalanceNumOfKills;
        public float lossContributionBalanceNumOfDeaths;
        public float lossContributionBalanceObjective1;
        public float lossContributionBalanceObjective2;
        public float lossContributionBalanceObjective3;	
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        Int32[] undefined3;
        public float pvpPointsTotalBalance;
        public float pvpPointsPerTeamBaseExp;
        public float pvpPointsPerTeamCommonExp;
        public float pvpPointPerIndividualContribution;
        public float pvpPointsPerPenaltyExp;
        public WinConditionPriority winConditionPriority1;
        public WinConditionPriority winConditionPriority2;
        public WinConditionPriority winConditionPriority3;
        public WinConditionPriority winConditionPriority4;
        public WinConditionPriority winConditionPriority5;

        public enum WinConditionPriority
        {
            Null = -1,
            Score = 0,
            Kills = 1,
            LevelSum = 2,
            NodeCount = 3,
            PlayersLeft = 4,
        }
    }
}
