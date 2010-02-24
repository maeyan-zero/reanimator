using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

namespace Reanimator.Excel
{
    public class GameGlobals : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GameGlobalsTable
        {
            TableHeader header;

            [ExcelTable.ExcelOutput(IsStringOffset = true)]
            public Int32 name;
            Int32 buffer;
            public Int32 intValue;
            public float floatValue;
        }

        public GameGlobals(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GameGlobalsTable>(data, ref offset, Count);
        }

        public override DataTable GetRowReferences()
        {
            DataTable table = new DataTable();

            table.Columns.Add("index", typeof(int));
            table.Columns.Add("alias", typeof(string));

            table.Rows.Add(0, "GameTicksPerSecond");
            table.Rows.Add(1, "MerchantRefreshTimeInTicks");
            table.Rows.Add(2, "MerchantRefreshRequiredTimeBetweenBrowseInTicks");
            table.Rows.Add(3, "AbsoluteLevelMax");
            table.Rows.Add(4, "DamageIncrementAtZeroEnergy");
            table.Rows.Add(5, "RadiusDivisor");
            table.Rows.Add(6, "ItemDistroMaxDistance");
            table.Rows.Add(7, "BaseAntiEvade");
            table.Rows.Add(8, "BaseAntiBlock");
            table.Rows.Add(9, "ScrapValuePercentOfItemSellPrice");
            table.Rows.Add(10, "CraftsmanRefreshTimeInMinutes");
            table.Rows.Add(11, "SkillPointsPerTier");
            table.Rows.Add(12, "SkillPointsPerTierToUnlock");
            table.Rows.Add(13, "TimeFromTruthFlashTillTransportInSeconds");
            table.Rows.Add(14, "TimeForAutoTalkAfterTruthTransportInSeconds");
            table.Rows.Add(15, "MaxPartyMembers");
            table.Rows.Add(16, "MaxItemUpgrades");
            table.Rows.Add(17, "MaxItemAugments");
            table.Rows.Add(18, "NPCQuestRefreshTicks");
            table.Rows.Add(19, "ArcLightningRange");
            table.Rows.Add(20, "ArcLightningMaxTargets");
            table.Rows.Add(21, "DualWeaponDamageIncrementMelee");
            table.Rows.Add(22, "DualWeaponDamageIncrementFocus");
            table.Rows.Add(23, "LevelUpdateDelayInTicks");
            table.Rows.Add(24, "RestrictedWarpTimeAfterJoinPartyInSeconds");
            table.Rows.Add(25, "CreditLineDisplayTimeFactor");
            table.Rows.Add(26, "CreditPageMinDisplayTimeInSeconds");
            table.Rows.Add(27, "CreditPageInitialDelayTimeInSeconds");
            table.Rows.Add(28, "CreditMaxLinesPerPage");
            table.Rows.Add(29, "CreditCharacterGroupDisplayTimeFactor");
            table.Rows.Add(30, "CreditNumCharactersPerDisplayTimeFactor");
            table.Rows.Add(31, "PassagewayMoneyChancePerPathNode");
            table.Rows.Add(32, "PassagewayItemChancePerPathNode");
            table.Rows.Add(33, "PassagewayMoneyAmountLevelMultiplierMin");
            table.Rows.Add(34, "PassagewatMoneyAmountLevelMultiplierMax");
            table.Rows.Add(35, "UIBarRegenInterval");
            table.Rows.Add(36, "EliteMonsterDamagePercent");
            table.Rows.Add(37, "EliteMonsterSFXChancePercent");
            table.Rows.Add(38, "ElitePlayerDamagePercent");
            table.Rows.Add(39, "ElitePlaterSFXChancePercent");
            table.Rows.Add(40, "EliteMonsterAwarenessIncrease");
            table.Rows.Add(41, "EliteChampionPercent");
            table.Rows.Add(42, "EliteMonsterSpeedPercent");
            table.Rows.Add(43, "EliteNanoForgeCostPercent");
            table.Rows.Add(44, "EliteAugmentCostPercent");
            table.Rows.Add(45, "PVPSFXChancePercent");
            table.Rows.Add(46, "EliteItemSellPricePercent");
            table.Rows.Add(47, "EliteMonsterDensityMultiplier");
            table.Rows.Add(48, "PartyEnterInstanceLevelDelayInTicks");
            table.Rows.Add(49, "AutoPartyUpdateDelayInTicks");
            table.Rows.Add(50, "AutoPartyMaxMemberCount");
            table.Rows.Add(51, "AutoPartyUpdateDelayInTicks");
            table.Rows.Add(52, "MapPriceMultiplier");
            table.Rows.Add(53, "MapPriceMultiplierEpic");
            table.Rows.Add(54, "MaxLuckHellgate");
            table.Rows.Add(55, "MaxLuckTugboat");
            table.Rows.Add(56, "ChanceForSocket");
            table.Rows.Add(57, "ChanceForRareSocket");
            table.Rows.Add(58, "PlayerVsPlayerDmgMod");
            table.Rows.Add(59, "BossRespawnDelay");

            return table;
        }

        public override DataTable GetColumnReferences()
        {
            DataTable table = new DataTable();

            table.Columns.Add("index", typeof(int));
            table.Columns.Add("alias", typeof(string));

            table.Rows.Add(2, "IntValue");
            table.Rows.Add(3, "FloatValue");

            return table;
        }
    }
}
