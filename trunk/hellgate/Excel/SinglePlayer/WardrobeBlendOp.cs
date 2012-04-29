using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBlendOp
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 replaceAllParts;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noTextureChange;//bool;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 removeParts10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_PART")]
        public Int32 addParts10;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string blend;
        short undefined1;
        [ExcelOutput(ConstantValue = -1)]
        Int32 undefined2;
        public Target target; // XLS_InternalIndex_TargetTexture (XLS_WARDROBE_BLENDOP_DATA+14B), 0x09
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_BLENDOP")]
        public Int32 covers10;

        public enum Target
        {
            Null = -1,
            Body_Right = 0,
            Head_Other_Left = 1,
            Cape = 2,
            Helmet = 3
        }
    }
}