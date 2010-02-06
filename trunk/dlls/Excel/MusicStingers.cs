using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MusicStingers : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicStingersTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 type;
            public Int32 fadeOutBeats;
            public Int32 fadeInBeats;
            public Int32 fadeInDelayBeats;
            public Int32 fadeOutDelayBeats;
            public Int32 introBeats;
            public Int32 outroBeats;
            public Int32 soundGroup;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] undefined;
        }

        public MusicStingers(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicStingersTable>(data, ref offset, Count);
        }
    }
}
