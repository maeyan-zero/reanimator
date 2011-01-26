using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DefenseGameWaveBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 monsterClass;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 spawnClass;
        public Int32 spawnTime;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 spawnMessage;
    }
}
