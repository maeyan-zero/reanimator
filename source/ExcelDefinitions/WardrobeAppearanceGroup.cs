using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeAppearanceGroupRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string blendTextureFolder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string textureFolder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string appearance;

        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 subscriberOnly;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noBodyParts;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 dontRandomlyPick;//bool;
        public Int32 blendOpGroup;
        public Int32 firstPerson;//idx;
        public Int32 Base1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
        Int32[] BaseUnused;
        public Int32 heads1;
        public Int32 heads2;
        public Int32 heads3;
        public Int32 heads4;
        public Int32 heads5;
        public Int32 heads6;
        public Int32 heads7;
        public Int32 heads8;
        public Int32 heads9;
        public Int32 heads10;
        public Int32 heads11;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        Int32[] headsUnused;
        public Int32 hair1;
        public Int32 hair2;
        public Int32 hair3;
        public Int32 hair4;
        public Int32 hair5;
        public Int32 hair6;
        public Int32 hair7;
        public Int32 hair8;
        public Int32 hair9;
        public Int32 hair10;
        public Int32 hair11;
        public Int32 hair12;
        public Int32 hair13;
        public Int32 hair14;
        public Int32 hair15;
        public Int32 hair16;
        public Int32 hair17;
        public Int32 hair18;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        Int32[] hairUnused;
        public Int32 facialHair1;
        public Int32 facialHair2;
        public Int32 facialHair3;
        public Int32 facialHair4;
        public Int32 facialHair5;
        public Int32 facialHair6;
        public Int32 facialHair7;
        public Int32 facialHair8;
        public Int32 facialHair9;
        public Int32 facialHair10;
        public Int32 facialHair11;
        public Int32 facialHair12;
        public Int32 facialHair13;
        public Int32 facialHair14;
        public Int32 facialHair15;
        public Int32 facialHair16;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] facialHairUnused;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        Int32[] skin;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        Int32[] hairColorTexture;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        Int32[] undefined1;
        public Int32 category;
        public Int32 section;
    }
}