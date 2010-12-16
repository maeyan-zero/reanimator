using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsRoomIndex
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 outDoor;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 outDoorVisibility;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 computeAmbientOcclusion;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noMonsterSpawn;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 noAdventures;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 noSubLevelEntrance;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 occupiesNodes;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 raisesNodes;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 fullCollision;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 dontObstructSound;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 dontOccludeVisibility;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 thirdPersonCameraIgnore;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 rtsCameraIgnore;//bool
        public Int32 havokSliceType;
        public Int32 roomVersion;
        public float nodeBuffer;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string reverbEnvironment;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined2;
        public Int32 backGroundSound;//idx
        public Int32 noGore;//idx;
        public Int32 noHumans;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined3;
    }
}
