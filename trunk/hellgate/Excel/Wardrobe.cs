﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Wardrobe
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2, RequiresDefault = true)]
        public Int32 code;
        public Int32 undefined1;
        public Int32 rowCollection;
        public Int32 order;
        [ExcelOutput(IsBool = true)]
        public Int32 debug;
        public Int32 randomAppearanceGroups1;
        public Int32 randomAppearanceGroups2;
        public Int32 randomAppearanceGroups3;
        public Int32 randomAppearanceGroups4;
        public Int32 randomAppearanceGroups5;
        public Int32 randomAppearanceGroups6;
        public Int32 randomAppearanceGroups7;
        public Int32 randomAppearanceGroups8;
        public Int32 randomAppearanceGroups9;
        public Int32 randomAppearanceGroups10;
        public Int32 layerSet1;
        public Int32 layerSet2;
        public Int32 layerSet3;
        public Int32 layerSet4;
        public Int32 layerSet5;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_MODEL_GROUP")]
        public Int32 modelGroup;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_TEXTURESET_GROUP")]
        public Int32 textureSetGroup;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_BLENDOP")]
        public Int32 blendOp;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_BLENDOP")]
        public Int32 blendOpTemplar;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_BLENDOP")]
        public Int32 blendOpCabalist;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_BLENDOP")]
        public Int32 blendOpHunter;//idx;
        [ExcelOutput(IsTableIndex = true, TableStringID = "WARDROBE_BLENDOP")]
        public Int32 blendOpAdventurer;//idx;
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
        [ExcelOutput(IsBool = true)]
        public Int32 hasBoneIndex;//bool
        public Int32 boneIndex;
        public Int32 state;//idx
    }
}