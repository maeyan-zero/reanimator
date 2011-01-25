using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpPerTimeBeta
    {
        RowHeader header;
        public Int32 elapsedTimeRate;
        public float experienceWeightDuel;
        public float experienceWeightTdm;
        public float experienceWeightElm;
        public float experienceWeightCtl;
        public float experienceWeightCtf;
        public float pvpPointWeightDuel;
        public float pvpPointWeightTdm;
        public float pvpPointWeightElm;
        public float pvpPointWeightCtl;
        public float pvpPointWeightCtf;
    }
}
