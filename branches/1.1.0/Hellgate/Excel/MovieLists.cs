using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MovieListsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 list1a;
        public Int32 list1b;
        public Int32 list1c;
        public Int32 list1d;
        public Int32 list1e;
        public Int32 list1f;
        public Int32 list1g;
        public Int32 list1h;
        public Int32 list2a;
        public Int32 list2b;
        public Int32 list2c;
        public Int32 list2d;
        public Int32 list2e;
        public Int32 list2f;
        public Int32 list2g;
        public Int32 list2h;
    }
}