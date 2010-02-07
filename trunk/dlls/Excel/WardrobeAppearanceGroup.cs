using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobeAppearanceGroup : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeAppearanceGroupTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string blendTextureFolder;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string textureFolder;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string appearance;

            public Int32 code;
            public Int32 subscriberOnly;//bool;
            public Int32 noBodyParts;//bool;
            public Int32 dontRandomlyPick;//bool;
            public Int32 blendOpGroup;
            public Int32 firstPerson;//idx;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] Base;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] heads;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] hair;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] facialHair;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] skin;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public Int32[] hairColorTexture;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            public Int32[] undefined1;
            public Int32 category;
            public Int32 section;
        }

        public WardrobeAppearanceGroup(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeAppearanceGroupTable>(data, ref offset, Count);
        }
    }
}
