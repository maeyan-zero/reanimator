using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpPvpPointPerProgressTimeBeta
    {
        RowHeader header;
        public Int32 progressTimeRate;
        public float experienceWeightProgressForDuel;
        public float experienceWeightProgressForTdm;
        public float experienceWeightProgressForElm;
        public float experienceWeightProgressForCtl;
        public float experienceWeightProgressForCtf;
        public float pvpPointWeightProgressForDuel;
        public float pvpPointWeightProgressForTdm;
        public float pvpPointWeightProgressForElm;
        public float pvpPointWeightProgressForCtl;
        public float pvpPointWeightProgressForCtf;
    }
}
