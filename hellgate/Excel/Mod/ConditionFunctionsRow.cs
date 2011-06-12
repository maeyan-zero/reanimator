using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ConditionFunctionsRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String function;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        byte[] undefined1;
        [ExcelOutput(IsBool = true)]
        public Int32 undefinedBool1;
        [ExcelOutput(IsBool = true)]
        public Int32 undefinedBool2;
        [ExcelOutput(IsBool = true)]
        public Int32 undefinedBool3;
        [ExcelOutput(IsBool = true)]
        public Int32 undefinedBool4;
        [ExcelOutput(IsBool = true)]
        public Int32 usesMonsterClass;
        [ExcelOutput(IsBool = true)]
        public Int32 usesObjectClass;
        [ExcelOutput(IsBool = true)]
        public Int32 usesStat;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String paramText0;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public String paramText1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined2;
    }
}
