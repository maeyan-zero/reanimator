using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DefenseGameMonsterBuffBeta
    {
        ExcelFile.RowHeader header;
        public Int32 defenseObjectDestroyCount;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 message;
    }
}
