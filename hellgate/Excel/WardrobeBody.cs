using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBody
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 layers10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 Base;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 head;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 hair;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 facialHair;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 skin;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_APPEARANCE_GROUP")]
        public Int32 hairColorTexture;//idx
        public byte skinColor;
        public byte hairColor;
        short undefined;
        [ExcelOutput(IsBool = true)]
        public Int32 randomize;//bool
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 randomLayerSets1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 randomLayerSets2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 randomLayerSets3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYERSET")]
        public Int32 randomLayerSets4;
    }
}
