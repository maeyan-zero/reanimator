using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ObjectTriggersRow
    {
        ExcelFile.TableHeader header;

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
        public Int32 deadCanTrigger;//bool
        public Int32 ghostCanTrigger;//bool
        public Int32 hardCoreDeadCanTrigger;//bool;
        public Int32 undefined2;
    }
}