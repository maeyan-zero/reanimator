using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBody
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 layers1;
        public Int32 layers2;
        public Int32 layers3;
        public Int32 layers4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        Int32[] layersUnused;
        public Int32 Base;//idx
        public Int32 head;//idx
        public Int32 hair;//idx
        public Int32 facialHair;//idx
        public Int32 skin;//idx
        public Int32 hairColorTexture;//idx
        public byte skinColor;
        public byte hairColor;
        short undefined;
        [ExcelOutput(IsBool = true)]
        public Int32 randomize;//bool
        public Int32 randomLayerSets1;
        public Int32 randomLayerSets2;
        public Int32 randomLayerSets3;
        Int32 randomLayerSets4;
    }
}
