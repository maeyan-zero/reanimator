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
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "TUTORIALSTRINGS")]//table C6 labeled Tutorials, perhaps really tutorialtrings
        public Int32 nextRow;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string linkData;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;
        public DataType dataType;
        public Int32 duration;
        public Int32 delay;
        public StartCondition startCondition;

        public enum DataType
        {
            Null = -1,
            String = 0,
            Communicator = 1,
            Image = 2
        }

        public enum StartCondition
        {
            Null = -1,
            Activate = 0,
            Complete = 1
        }

    }
}
