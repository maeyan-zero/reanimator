using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RecipeListsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string recipeList;

        public Int32 code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] recipes;
        public Int32 randomlySelectable;//bool
        public Int32 randomSelectWeight;

    }
}