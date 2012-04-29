using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RecipeLists
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipeList;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "RECIPES")]
        public Int32 recipes8;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 recipes9;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 recipes10;
        [ExcelOutput(IsBool = true, DebugIgnoreConstantCheck = true)]
        public Int32 randomlySelectable;//bool
        [ExcelOutput(IsBool = true, DebugIgnoreConstantCheck = true)]
        public Int32 randomSelectWeight;//bool
    }
}