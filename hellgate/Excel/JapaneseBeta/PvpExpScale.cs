using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpScaleBeta
    {
        RowHeader header;
        public Int32 difference;
        public Int32 levelExpScalePct;
        public Int32 rankExpScalePct;
        public Int32 pvpRankExpScalePct;
    }
}
