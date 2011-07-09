using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class WardrobeAppearanceGroup
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string blendTextureFolder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string textureFolder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string appearance;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 subscriberOnly;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noBodyParts;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 dontRandomlyPick;//bool;
        public BlendOpGroup blendOpGroup; // XLS_InternalIndex_BlendOpGroup (XLS_WARDROBE_APPEARANCE_GROUP_DATA+16C), 0x06
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 firstPerson;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int32[] baseGroup;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 base2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 base3;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] baseUnused;
         
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public Int32[] heads;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads4;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads5;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads6;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads7;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads8;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads9;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads10;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 heads11;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        Int32[] headsUnused;

        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public Int32[] hair;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair4;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair5;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair6;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair7;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair8;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair9;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair10;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair11;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair12;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair16;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair17;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hair18;
        //[ExcelOutput(ConstantValue = -1)]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        //Int32[] hairUnused;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
        public Int32[] facialHair;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair2;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair3;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair4;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair5;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair6;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair7;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair8;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair9;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair10;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair11;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair12;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair13;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair14;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair15;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 facialHair16;
        //[ExcelOutput(ConstantValue = -1)]
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        //Int32[] facialHairUnused;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int32[] skin;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 skin2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        Int32[] skinUnused;

        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public Int32[] hairColorTexture;
        //[ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        //public Int32 hairColorTexture2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
        Int32[] hairColorTextureUnused;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
        Int32[] undefined1;
        public Category category; // XLS_InternalIndex_AppearanceGroupCategory (XLS_WARDROBE_APPEARANCE_GROUP_DATA+F0), 0x0D
        public Section section; // XLS_InternalIndex_Section (XLS_WARDROBE_APPEARANCE_GROUP_DATA+132), 0x08

        public enum BlendOpGroup
        {
            Default = 0,
            Templar = 1,
            Cabalist = 2,
            Hunter = 3,
            Adventurer = 4
        }

        public enum Category
        {
            None = -2,
            Ignore = -1,
            Zombie = 0,
            Minion = 1,
            ThreeP_Gender = 2,
            ThreeP_Faction = 3,
            OneP_Gender = 4,
            OneP_Faction = 5,
            Body = 6,
            Head = 7,
            Hair = 8,
            FaceExtras = 9
        }

        public enum Section
        {
            None = -2,
            Null = -1,
            Base = 0,
            Head = 1,
            Hair = 2,
            FacialHair = 3,
            Skin = 4,
            Haircolor = 5
        }
    }
}