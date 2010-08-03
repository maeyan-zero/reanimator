using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestTemplateRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 updatePartyKillOnSetup; //boolean
        [ExcelOutput(IsBool = true)]
        public Int32 removeOnJoinGame; //boolean
        public Int32 undefined1;
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