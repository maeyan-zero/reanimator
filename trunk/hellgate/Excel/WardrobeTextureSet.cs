using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeTextureSet
    {
        TableHeader header;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_TEXTURESET_GROUP", SortAscendingID = 1, SortColumnTwo = "appearanceGroup1")]
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
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        //public Int32[] undefinedBool;
        public Int32 undefinedBool1;
        public Int32 undefinedBool2;
        public Int32 undefinedBool3;
        public Int32 undefinedBool4;
        public Int32 undefinedBool5;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        //public Int32[] undefinedInt;
        public Int32 undefinedInt1;
        public Int32 undefinedInt2;
        public Int32 undefinedInt3;
        public Int32 undefinedInt4;
        public Int32 undefinedInt5;
        public Int32 undefinedInt6;
        public Int32 undefinedInt7;
        public Int32 undefinedInt8;
        public Int32 undefinedInt9;
        public Int32 undefinedInt10;
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
        public Int32 undefined2;
        public Int32 undefined2a;
    }
}