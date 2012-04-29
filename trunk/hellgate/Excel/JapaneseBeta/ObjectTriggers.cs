using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ObjectTriggersBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 clearTrigger;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        Int32[] undefined1;
        [ExcelOutput(IsBool = true)]
        public Int32 isWarp;
        [ExcelOutput(IsBool = true)]
        public Int32 isSubLevelWarp;
        [ExcelOutput(IsBool = true)]
        public Int32 isDoor;
        [ExcelOutput(IsBool = true)]
        public Int32 isBlocking;
        [ExcelOutput(IsBool = true)]
        public Int32 isDynamicWarp;
        [ExcelOutput(IsBool = true)]
        public Int32 allowsInvalidDestination;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 stateBlocking;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 stateOpen;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 deadCanTrigger;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 ghostCanTrigger;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 hardCoreDeadCanTrigger;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 pvpMachine;
        [ExcelOutput(IsBool = true)]
        public Int32 marketPlaceTerminal;
        [ExcelOutput(IsBool = true)]
        public Int32 stash;
        [ExcelOutput(IsBool = true)]
        public Int32 waypointTerminal;
        public Int32 undefined2;
    }
}
