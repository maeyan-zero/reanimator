using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TutorialStringsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "QUEST")]//table 9E
        public Int32 relatedQuest;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        //[ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "TUTORIALS")]//table C6 - No file in the dats, or exe, by that name
        public Int32 nextRow;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string linkData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;
        public Int32 dataType;
        public Int32 duration;
        public Int32 delay;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYER_CONDITION")]
        public Int32 startCondition;
    }
}
