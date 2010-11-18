using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeTCv4Row
    {
        ExcelFile.TableHeader header;

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
        public Int32 modelGroup;//idx;
        public Int32 textureSetGroup;//idx;
        public Int32 blendOp;//idx;
        public Int32 blendOpTemplar;//idx;
        public Int32 blendOpCabalist;//idx;
        public Int32 blendOpHunter;//idx;
        public Int32 blendOpSatyr;//idx;
        public Int32 blendOpCyclops;
        public Int32 blendOpCyclopsFemale;
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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        Int32[] undefined6;
        public Int32 undefined_tcv4_1;
        public Int32 undefined_tcv4_2;
        public Int32 undefined_tcv4_3;
        [ExcelOutput(IsBool = true)]
        public Int32 hasBoneIndex;//bool
        public Int32 boneIndex;
        public Int32 state;//idx
    }
}