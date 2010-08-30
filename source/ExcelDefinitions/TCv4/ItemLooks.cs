using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooksTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        public Int32 item;//idx
        public Int32 lookGroup;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string folder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
        public Int32 wardrobe;//idx;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        Int32[] TCV4_1;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        Int32[] TCV4_2;
    }
}