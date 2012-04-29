using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MiniGameBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 requiredUnitType;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 condition;
        public Int32 weight;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MINIGAME_TAG")]//table C4
        public Int32 tagName0;
        public Int32 overrideMinNeeded0;
        public Int32 overrideMaxNeeded0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MINIGAME_TAG")]//table C4
        public Int32 tagName1;
        public Int32 overrideMinNeeded1;
        public Int32 overrideMaxNeeded1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "MINIGAME_TAG")]//table C4
        public Int32 tagName2;
        public Int32 overrideMinNeeded2;
        public Int32 overrideMaxNeeded2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 sound;
    }
}
