using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PlayerRanksBeta
    {
        RowHeader header;
        public Int32 unitType;
        public Int32 level;
        public Int32 rankExperience;
        public Int32 perkPoints;
        public Int32 rank_stat_points;
    }
}
