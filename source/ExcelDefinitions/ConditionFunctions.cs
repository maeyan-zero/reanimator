using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ConditionFunctionsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefinedBool;
        public Int32 usesMonsterClass;//bool
        public Int32 usesObjectClass;//bool
        public Int32 usesStat;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramText0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramText1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined2;
    }
}