using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SoundBuses : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SoundBusesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 volume;
            public Int32 undefined;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string effects;
            public Int32 sendsTo;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public Int32[] vca;
            public Int32 userControl;
        }

        public SoundBuses(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SoundBusesTable>(data, ref offset, Count);
        }
    }
}
