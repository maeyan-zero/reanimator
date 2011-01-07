using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Wardrobe
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;                                                                 // 0x00
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;                                                                  // 0x40
        public Int32 undefined1;
        public Int32 rowCollection;                                                         // 0x48
        public Int32 order;                                                                 // 0x4C
        [ExcelOutput(IsBool = true)]
        public Int32 debug;                                                                 // 0x50
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups1;                                               // 0x54
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups2;                                               // 0x58
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups3;                                               // 0x5C
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups4;                                               // 0x60
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups5;                                               // 0x64
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups6;                                               // 0x68
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups7;                                               // 0x6C
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups8;                                               // 0x70
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups9;                                               // 0x74
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 randomAppearanceGroups10;                                              // 0x78
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet1;                                                             // 0x7C
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet2;                                                             // 0x80
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet3;                                                             // 0x84
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 layerSet4;                                                             // 0x88
        Int32 undefined3; // is all zero; not defined in ASM
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_MODEL_GROUP")]
        public Int32 modelGroup;//idx;                                                      // 0x90
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_TEXTURESET_GROUP")]
        public Int32 textureSetGroup;//idx;                                                 // 0x94
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOp;//idx;                                                         // 0x98
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpTemplar;//idx;                                                  // 0x9C
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpCabalist;//idx;                                                 // 0xA0
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 blendOpHunter;//idx;                                                   // 0xA4
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP", ConstantValue = -1)]
        Int32 blendOpAdventurer;//idx;                                                      // 0xA8
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 offHandlayer;//idx;                                                    // 0xAC
        public Int32 undefined2;
        public Int32 attachType; // XLS_InternalIndex_AttachType (XLS_WARDROBE_LAYER_DATA+265), 0x05    // 0xB4
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string attachName;                                                           // 0xC0
        public Int32 undefined5;                                                            // 0x1C0 (null name)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string attachBone;                                                           // 0x1C4
        public Int32 undefined6;                                                            // 0x2C4 (null name)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        Int32[] undefined7;
        [ExcelOutput(IsBool = true)]
        public Int32 hasBoneIndex;//bool                                                    // 0x2F0
        public Int32 boneIndex; // XLS_InternalIndex_BoneIndex (XLS_WARDROBE_LAYER_DATA+231), 0x07      // 0x2F4
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx                                                            // 0x2F8
    }
}