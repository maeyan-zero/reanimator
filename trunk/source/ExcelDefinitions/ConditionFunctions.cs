using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ConditionFunctionsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefinedBool;
        [ExcelOutput(IsBool = true)]
        public Int32 usesMonsterClass;
        [ExcelOutput(IsBool = true)]
        public Int32 usesObjectClass;
        [ExcelOutput(IsBool = true)]
        public Int32 usesStat;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramText0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string paramText1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined2;
    }
}