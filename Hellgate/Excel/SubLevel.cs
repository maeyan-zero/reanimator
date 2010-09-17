using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SubLevelRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 drlg;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 allowTownPortals;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 allowMonsterDistribution;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 headStoneAtEntranceObject;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 respawnAtEntrance;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 partyPortalsAtEntrance;//bool
        public Int32 type;
        public float defaultPositionX;
        public float defaultPositionY;
        public float defaultPositionZ;
        public float entranceFlatZTolerance;
        public float entranceFlatRadius;
        public float entranceFlatHeightMin;
        [ExcelOutput(IsBool = true)]
        public Int32 autoCreateEntrance;//bool
        public Int32 objectEntrance;//idx
        public Int32 objectExit;//idx
        public Int32 alternativeEntranceUnitType;//idx
        public Int32 alternativeExitUnitType;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 allowLayoutMarkersForEntrance;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string layoutMarkerEntranceName;
        [ExcelOutput(IsBool = true)]
        public Int32 allowPathNodesForEntrance;//bool
        public Int32 subLevelNext;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 overRideLevelSpawns;//bool
        public Int32 spawnClass;//idx
        public Int32 weather;//idx
        public Int32 stateWhenInsideSubLevel;//idx
    }
}