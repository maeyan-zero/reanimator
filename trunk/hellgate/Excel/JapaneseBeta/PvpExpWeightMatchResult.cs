using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpWeightMatchResultBeta
    {
        RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String gameResult;
        public float teamWeightDuel;
        public float teamWeightTdm;
        public float teamWeightElm;
        public float teamWeightCtl;
        public float teamWeightCtf;
        public float teamGeneralWeight;
        public float individualWeightDuel;
        public float individualWeightTdm;
        public float individualWeightElm;
        public float individualWeightCtl;
        public float individualWeightCtf;
    }
}
