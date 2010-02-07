using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Wardrobe : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 undefined1;
            public Int32 rowCollection;
            public Int32 order;
            public Int32 debug;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] randomAppearanceGroups;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public Int32[] layerSet;
            public Int32 modelGroup;//idx;
            public Int32 textureSetGroup;//idx;
            public Int32 blendOp;//idx;
            public Int32 blendOpTemplar;//idx;
            public Int32 blendOpCabalist;//idx;
            public Int32 blendOpHunter;//idx;
            public Int32 blendOpAdventurer;//idx;
            public Int32 offHandlayer;//idx;
            public Int32 undefined2;
            public Int32 attachType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public Int32[] undefined4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string attachName;
            public Int32 undefined5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string attachBone;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public Int32[] undefined6;
            public Int32 hasBoneIndex;//bool
            public Int32 boneIndex;
            public Int32 state;//idx
        }

        public Wardrobe(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeTable>(data, ref offset, Count);
        }
    }
}
