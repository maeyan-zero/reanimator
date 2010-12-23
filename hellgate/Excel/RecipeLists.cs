using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RecipeLists
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipeList;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 recipes1;
        public Int32 recipes2;
        public Int32 recipes3;
        public Int32 recipes4;
        public Int32 recipes5;
        public Int32 recipes6;
        public Int32 recipes7;
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