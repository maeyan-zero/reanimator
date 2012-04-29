using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpExpPerEnemyBeta
    {
        RowHeader header;
        public Int32 numberOfEnemies;
        public Int32 experience_Duel;
        public Int32 experience_Tdm;
        public Int32 experience_Elm;
        public Int32 experience_Ctl;
        public Int32 experience_Ctf;
    }
}
