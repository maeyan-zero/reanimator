using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SpawnClass : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SpawnClassTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string spawnClass;

            public Int32 code;
            public Int32 pickType;
            public Int32 rememberPick;//bool
            public Int32 pick1;
            public Int32 undefined1a;
            public Int32 count1;//intptr
            public Int32 undefined1b;
            public Int32 weight1;
            public Int32 pick2;
            public Int32 undefined2a;
            public Int32 count2;//intptr
            public Int32 undefined2b;
            public Int32 weight2;
            public Int32 pick3;
            public Int32 undefined3a;
            public Int32 count3;//intptr
            public Int32 undefined3b;
            public Int32 weight3;
            public Int32 pick4;
            public Int32 undefineda;
            public Int32 count4;//intptr
            public Int32 undefined4b;
            public Int32 weight4;
            public Int32 pick5;
            public Int32 undefined5a;
            public Int32 count5;//intptr
            public Int32 undefined5b;
            public Int32 weight5;
            public Int32 pick6;
            public Int32 undefined6a;
            public Int32 count6;//intptr
            public Int32 undefined6b;
            public Int32 weight6;
            public Int32 pick7;
            public Int32 undefined7a;
            public Int32 count7;//intptr
            public Int32 undefined7b;
            public Int32 weight7;
            public Int32 pick8;
            public Int32 undefined8a;
            public Int32 count8;//intptr
            public Int32 undefined8b;
            public Int32 weight8;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            Int32[] undefined9;
        }

        public SpawnClass(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SpawnClassTable>(data, ref offset, Count);
        }
    }
}
