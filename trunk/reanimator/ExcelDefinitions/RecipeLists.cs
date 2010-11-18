using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RecipeListsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipeList;

        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        public Int32 recipes1;
        public Int32 recipes2;
        public Int32 recipes3;
        public Int32 recipes4;
        public Int32 recipes5;
        public Int32 recipes6;
        public Int32 recipes7;
        public Int32 recipes8;
        public Int32 recipes9;
        public Int32 recipes10;
        [ExcelOutput(IsBool = true)]
        public Int32 randomlySelectable;//bool
        public Int32 randomSelectWeight;

    }
}