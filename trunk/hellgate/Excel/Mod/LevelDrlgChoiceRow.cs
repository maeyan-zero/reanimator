using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelDrlgChoiceRow
    {
        RowHeader header;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 nameOffset; // is "name", but renamed to "nameOffset" so we can have "name" as string value
        public String name;
        public float namedMonsterChance;
        public Int32 undefined2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL")]
        public Int32 levelName;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "DIFFICULTY")]
        public Int32 difficulty;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 drlg;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 spawnClass;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 namedMonsterClass;//idx
        public Int32 weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSIC")]
        public Int32 music;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_ENVIRONMENTS")]
        public Int32 environmentOverRide;//idx
        public float environmentSpawnClassRoomDensity;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SPAWN_CLASS")]
        public Int32 environmentSpawnClass;//idx
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MONSTERS")]
        public Int32 eventSpawnMonsterClass;//idx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table 76
        public Int32 eventSpawnMOnsterQuality;//idx
        public Int32 eventSpawnMaxKillCount;
    }
}
