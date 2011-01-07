using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsThemes
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 isA7;
        [ExcelOutput(IsBool = true)]
        public Int32 dontDisplayInEditor;
        [ExcelOutput(IsBool = true)]
        public Int32 highLander;//is this supposed to be "there can be only one" highlander?
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string environment;
        public Int32 envPriority;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles10;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles11;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles12;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles13;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles14;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_DRLGS")]
        public Int32 allowedStyles15;
        [ExcelOutput(IsTableIndex = true, TableStringId = "GLOBAL_THEMES")]
        public Int32 globalThemeRequired;//idx
    }
}