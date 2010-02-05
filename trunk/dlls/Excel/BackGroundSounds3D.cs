using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BackGroundSounds3D : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BackGroundSounds3DTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 front;//bool
            public Int32 left;//bool
            public Int32 right;//bool
            public Int32 back;//bool
            public Int32 undefined1;
            public Int32 undefined2;
            public Int32 undefined3;
            public float minVolume;
            public float maxVolume;
            public float minIntersectDelay;
            public float maxIntersectDelay;
            public Int32 minSetCount;
            public Int32 maxSetCount;
            public float minIntrasetDelay;
            public float maxIntrasetDelay;
            public float setChance;
            public Int32 sound;//idx
        }

        public BackGroundSounds3D(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BackGroundSounds3DTable>(data, ref offset, Count);
        }
    }
}
