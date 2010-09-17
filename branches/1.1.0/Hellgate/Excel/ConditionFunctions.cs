using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ConditionFunctions
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        Byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public Int32[] undefinedBool;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesMonsterClass;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesObjectClass;
        [ExcelAttribute(IsBool = true)]
        public Int32 usesStat;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String paramText0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String paramText1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Byte[] undefined2;
    }
}