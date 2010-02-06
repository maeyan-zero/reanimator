using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MusicRef : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicRefTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 undefined1;
            public float undefined2;
            public float undefined3;
            public Int32 beatsPerMeasure;
            public Int32 offsetInMilliSeconds;
            public Int32 grooveUpdateInMeasures;
            public Int32 defaultGroove;//idx
            public Int32 offGoove;//idx
            public Int32 undefined4;
            public Int32 soundGroup;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            Int32[] undefined;
        }

        public MusicRef(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicRefTable>(data, ref offset, Count);
        }
    }
}
