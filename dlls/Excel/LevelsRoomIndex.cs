using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LevelsRoomIndex : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsRoomIndexTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 outDoor;//bool;
            public Int32 outDoorVisibility;//bool;
            public Int32 computeAmbientOcclusion;//bool;
            public Int32 noMonsterSpawn;//bool
            public Int32 noAdventures;//bool
            public Int32 noSubLevelEntrance;//bool
            public Int32 occupiesNodes;//bool
            public Int32 raisesNodes;//bool
            public Int32 fullCollision;//bool
            public Int32 dontObstructSound;//bool
            public Int32 dontOccludeVisibility;//bool
            public Int32 thirdPersonCameraIgnore;//bool
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

        public LevelsRoomIndex(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsRoomIndexTable>(data, ref offset, Count);
        }
    }
}
