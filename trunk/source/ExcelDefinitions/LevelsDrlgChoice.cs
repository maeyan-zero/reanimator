using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsDrlgChoiceRow
    {
        ExcelFile.TableHeader header;

        [ExcelFile.ExcelOutput(IsStringOffset = true)]
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
}