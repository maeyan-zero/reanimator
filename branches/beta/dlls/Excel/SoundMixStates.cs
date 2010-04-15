using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SoundMixStates : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SoundMixStatesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 priority;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] values;
            public float fadeInTimeInSeconds;
            public Int32 undefined1;
            public float fadeOutTimeInSeconds;
            public Int32 undefined2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string reverbOverRide;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] undefined3;
        }

        public SoundMixStates(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SoundMixStatesTable>(data, ref offset, Count);
        }
    }
}
