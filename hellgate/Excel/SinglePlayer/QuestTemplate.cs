using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestTemplate
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 updatePartyKillOnSetup; //boolean
        [ExcelOutput(IsBool = true)]
        Int32 removeOnJoinGame; //boolean // is always 0
        Int32 undefined1; // is always 0
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string initFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string freeFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined3;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string onEnterGameFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined4;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string completeFunction;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined5;
    }
}
