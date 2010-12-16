using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ObjectTriggers
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 clearTrigger;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined1;
        public Int32 isWarp;
        public Int32 isSubLevelWarp;
        public Int32 isDoor;
        public Int32 isBlocking;
        public Int32 isDynamicWarp;
        public Int32 allowsInvalidDestination;
        public Int32 stateBlocking;//idx
        public Int32 stateOpen;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 deadCanTrigger;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 ghostCanTrigger;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 hardCoreDeadCanTrigger;//bool;
        public Int32 undefined2;
    }
}
