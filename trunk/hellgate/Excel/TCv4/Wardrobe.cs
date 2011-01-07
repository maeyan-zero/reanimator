using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeTCv4
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 undefined1;
        public Int32 rowCollection;
        public Int32 order;
        [ExcelOutput(IsBool = true)]
        public Int32 debug;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet4;
        Int32 undefined3; // is all zero; not defined in ASM
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_MODEL_GROUP")]
        public Int32 modelGroup;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_TEXTURESET_GROUP")]
        public Int32 textureSetGroup;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOp;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpTemplar;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpCabalist;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpHunter;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpSatyr;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpCyclops;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpCyclopsFemale;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 offHandlayer;//idx;
        public Int32 undefined2;
        public Int32 attachType;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string attachName;
        public Int32 undefined5;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string attachBone;
        public Int32 undefined6;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        Int32[] undefined7;
        public Int32 undefined_tcv4_1;
        public Int32 undefined_tcv4_2;
        public Int32 undefined_tcv4_3;
        [ExcelOutput(IsBool = true)]
        public Int32 hasBoneIndex;//bool
        public Int32 boneIndex;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx
    }
}