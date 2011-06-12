using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LevelThemesRow
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public Int32[] isA;
        [ExcelOutput(IsBool = true)]
        public Int32 dontDisplayInEditor;
        [ExcelOutput(IsBool = true)]
        public Int32 highLander;//is this supposed to be "there can be only one" highlander?
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string environment;
        public Int32 envPriority;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Int32[] allowedStyles;
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;//idx
    }
}