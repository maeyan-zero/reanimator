using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LevelsDrlgChoice : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsDrlgChoiceTable
        {
            TableHeader header;

            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 name; //pchar
            public Int32 undefined1;
            public float namedMonsterChance;
            public Int32 undefined2;
            public Int32 levelName;//idx
            public Int32 difficulty;//idx
            public Int32 drlg;//idx
            public Int32 spawnClass;//idx
            public Int32 namedMonsterClass;//idx
            public Int32 weight;
            public Int32 music;//idx
            public Int32 environmentOverRide;//idx
            public float environmentSpawnClassRoomDensity;
            public Int32 environmentSpawnClass;//idx
            public Int32 code;
            public Int32 undefined3;
        }

        public LevelsDrlgChoice(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsDrlgChoiceTable>(data, ref offset, Count);
        }
    }
}
