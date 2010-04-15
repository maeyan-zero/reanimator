using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobeModel : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeModelTable
        {
            TableHeader header;

            public Int32 undefined1;
            public Int32 modelGroup;//idx;
            public Int32 appearanceGroup;//idx;
            public Int32 appearanceGroup2;//idx;
            public Int32 undefined2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string fileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string defaultMaterial;
            public Int32 undefined3;
            public Int32 partGroup;
            public Int32 undefinedBool1;
            public Int32 undefinedBool2;
            public Int32 undefinedBool3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] undefined4;
            public float boxMinX;
            public float boxMinY;
            public float boxMinZ;
            public float boxMaxX;
            public float boxMaxY;
            public float boxMaxZ;
        }

        public WardrobeModel(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeModelTable>(data, ref offset, Count);
        }
    }
}
