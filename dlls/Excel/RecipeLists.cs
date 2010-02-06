using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class RecipeLists : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class RecipeListsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string recipeList;

            public Int32 code;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] recipes;
            public Int32 randomlySelectable;//bool
            public Int32 randomSelectWeight;
            
        }

        public RecipeLists(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<RecipeListsTable>(data, ref offset, Count);
        }
    }
}
