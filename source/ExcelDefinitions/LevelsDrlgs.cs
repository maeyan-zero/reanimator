using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsDrlgsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string drlgRuleSet;
        public Int32 drlgDisplayName;//stridx
        public Int32 undefined1;
        [ExcelFile.ExcelOutputAttribute(IsStringIndex = true)]
        public Int32 style;
        public Int32 theme0;
        public Int32 theme1;
        public Int32 theme2;
        public Int32 theme3;
        public Int32 theme4;
        public Int32 theme5;
        public Int32 theme6;
        public Int32 theme7;
        public Int32 theme8;
        public Int32 theme9;
        public Int32 theme10;
        public Int32 theme11;
        public Int32 theme12;
        public Int32 theme13;
        public Int32 theme14;
        public Int32 theme15;
        public Int32 theme16;
        public Int32 theme17;
        public Int32 theme18;
        public Int32 theme19;
        public Int32 weatherSet;//idx
        public Int32 environment;//idx
        public Int32 populateAllVisible;//bool
        public float overrideChampionChance;
        public float championZoneRadius;
        public float markerSpawnChance;
        public float monsterRoomDensity;
        public Int32 roomMonsterChance;
        public Int32 critterRoomDensity;
        public Int32 critterSpawnClass;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined2;
        public float maxTreasureDropRadius;
        public Int32 randomState;//idx
        public Int32 randomStateRateMin;
        public Int32 randomStateRateMax;
        public Int32 randomStateRateDuration;
        public Int32 havokFx;//bool
        public Int32 forceDrawAllRooms;//bool
        public Int32 isOutDoors;//bool

    }
}