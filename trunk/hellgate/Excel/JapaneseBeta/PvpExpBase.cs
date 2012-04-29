using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpBaseBeta
    {
        RowHeader header;
        public Int32 statValue;
        public Int32 levelExpValue;
        public Int32 rankExpValue;
        public Int32 pvpRankExpValue;
    }
}
