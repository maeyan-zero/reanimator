using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemUpgradeBeta
    {
        ExcelFile.RowHeader header;
        public Int32 upgrades;
        public Int32 damageMult;
        public Int32 shields;
        public Int32 shieldBuffer;
        public Int32 shieldRegen;
        public Int32 armor;
        public Int32 armorBuffer;
        public Int32 armorRegen;
        public Int32 feed;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 requiredMoney;
        public Int32 requiredNanoshardsa;
        public Int32 successRate;
        public Int32 scrapUpgradeAdder;
        public Int32 specialScrapUpgradeAdder;
        public Int32 nanoshardDropRateForNormal;
        public Int32 nanoshardDropRateForUncommon;
        public Int32 nanoshardDropRateForRare;
        public Int32 nanoshardDropRateForLegendary;
        public Int32 nanoshardDropRateForUnique;
        public Int32 nanoshardDropRateForMythic;
        public Int32 nanoshardDropRateForSet;
        public Int32 minNanoshardForNormal;
        public Int32 minNanoshardForUncommon;
        public Int32 minNanoshardForRare;
        public Int32 minNanoshardForLegendary;
        public Int32 minNanoshardForUnique;
        public Int32 minNanoshardForMythic;
        public Int32 minNanoshardForSet;
        public Int32 maxNanoshardForNormal;
        public Int32 maxNanoshardForUncommon;
        public Int32 maxNanoshardForRare;
        public Int32 maxNanoshardForLegendary;
        public Int32 maxNanoshardForUnique;
        public Int32 maxNanoshardForMythic;
        public Int32 maxNanoshardForSet;
		
    }
}
