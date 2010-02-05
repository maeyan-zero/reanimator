using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BackGroundSounds2D : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BackGroundSounds2DTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public float undefined1;
            public float undefined2;
            public float minVolume;
            public float maxVolume;
            public float silentChance;
            public float minPlayTime;
            public float maxPlayTime;
            public float minSilentTime;
            public float maxSilentTime;
            public float fadeIn;
            public float fadeOut;
            public Int32 sound;//idx
        }

        public BackGroundSounds2D(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BackGroundSounds2DTable>(data, ref offset, Count);
        }
    }
}
