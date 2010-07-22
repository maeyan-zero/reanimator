using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeTextureSetRow
    {
        ExcelFile.TableHeader header;

        public Int32 textureSetGroup;//idx;
        public Int32 appearanceGroupFolder;//idx;
        public Int32 appearanceGroup1;//idx;
        public Int32 appearanceGroup2;//idx;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string folder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string diffuse;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string normal;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string specular;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string lightMap;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string colorMask;
        public short undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int32[] undefinedBool;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] undefinedInt;
        public Int32 sizeDiffuseW;//Width
        public Int32 sizeDiffuseH;//Height
        public Int32 sizeNormalW;
        public Int32 sizeNormalH;
        public Int32 sizeSpecularW;
        public Int32 sizeSpecularH;
        public Int32 sizeLightMapW;
        public Int32 sizeLightMapH;
        public Int32 sizeColorMaskW;
        public Int32 sizeColorMaskH;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public Int32[] undefined2;
    }
}