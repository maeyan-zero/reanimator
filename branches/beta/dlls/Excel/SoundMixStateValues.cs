using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SoundMixStateValues : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SoundMixStateValuesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 undefined1;
            public Int32 busVolume;
            public float undefined2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string effects;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public Int32[] undefined3;
        }

        public SoundMixStateValues(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SoundMixStateValuesTable>(data, ref offset, Count);
        }
    }
}
