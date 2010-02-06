using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SubLevel : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SubLevelTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 drlg;//idx
            public Int32 allowTownPortals;//bool
            public Int32 allowMonsterDistribution;//bool
            public Int32 headStoneAtEntranceObject;//bool
            public Int32 respawnAtEntrance;//bool
            public Int32 partyPortalsAtEntrance;//bool
            public Int32 type;
            public float defaultPositionX;
            public float defaultPositionY;
            public float defaultPositionZ;
            public float entranceFlatZTolerance;
            public float entranceFlatRadius;
            public float entranceFlatHeightMin;
            public Int32 autoCreateEntrance;//bool
            public Int32 objectEntrance;//idx
            public Int32 objectExit;//idx
            public Int32 alternativeEntranceUnitType;//idx
            public Int32 alternativeExitUnitType;//idx
            public Int32 allowLayoutMarkersForEntrance;//bool
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string layoutMarkerEntranceName;
            public Int32 allowPathNodesForEntrance;//bool
            public Int32 subLevelNext;//idx
            public Int32 overRideLevelSpawns;//bool
            public Int32 spawnClass;//idx
            public Int32 weather;//idx
            public Int32 stateWhenInsideSubLevel;//idx
        }

        public SubLevel(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SubLevelTable>(data, ref offset, Count);
        }
    }
}
