using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsRoomIndexBeta
    {
        RowHeader header;                                           //                  0x00     0
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;                                         // 0x00     0       0x10     16
        [ExcelOutput(IsBool = true)]
        public Int32 outDoor;//bool;                                // 0x40     64      0x50     80
        [ExcelOutput(IsBool = true)]
        public Int32 outDoorVisibility;//bool;                      // 0x44     68      0x54     84
        [ExcelOutput(IsBool = true)]
        public Int32 computeAmbientOcclusion;//bool;                // 0x48     72      0x58     88
        [ExcelOutput(IsBool = true)]
        public Int32 noMonsterSpawn;//bool                          // 0x4C     76
        [ExcelOutput(IsBool = true)]
        public Int32 noAdventures;//bool                            // 0x50     80
        [ExcelOutput(IsBool = true)]
        public Int32 noSubLevelEntrance;//bool                      // 0x54     84
        [ExcelOutput(IsBool = true)]
        public Int32 occupiesNodes;//bool                           // 0x58     88
        [ExcelOutput(IsBool = true)]
        public Int32 raisesNodes;//bool                             // 0x5C     92
        [ExcelOutput(IsBool = true)]
        public Int32 fullCollision;//bool                           // 0x60     96
        [ExcelOutput(IsBool = true)]
        public Int32 dontObstructSound;//bool                       // 0x64     100
        [ExcelOutput(IsBool = true)]
        public Int32 dontOccludeVisibility;//bool                   // 0x68     104
        [ExcelOutput(IsBool = true)]
        public Int32 thirdPersonCameraIgnore;//bool                 // 0x6C     108
        [ExcelOutput(IsBool = true)]
        public Int32 rtsCameraIgnore;//bool                         // 0x70     112
        public Int32 havokSliceType;                                // 0x74     116
        public Int32 roomVersion;                                   // 0x78     120
        public float nodeBuffer;                                    // 0x7C     124
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;                                         // 0x80     128
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string reverbEnvironment;                            // 0x88     136
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined2;                                         // 0x188    392
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS")]
        public Int32 backGroundSound;//idx                          // 0x190    400
        [ExcelOutput(IsTableIndex = true, TableStringId = "PROPS")]
        public Int32 noGore;//idx;                                  // 0x194    404
        [ExcelOutput(IsTableIndex = true, TableStringId = "PROPS")]
        public Int32 noHumans;//idx                                 // 0x198    408
		public Int32 ppl_folderCode;//code?
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string link;		
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined3;                                         // 0x19C    412
        // end of struct                                            // 0x1A8    424
    }
}
