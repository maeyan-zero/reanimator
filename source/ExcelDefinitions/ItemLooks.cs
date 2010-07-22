using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooksRow
    {
        ExcelFile.TableHeader header;

        public Int32 item;//idx
        public Int32 lookGroup;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string folder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
        public Int32 wardrobe;//idx;
        public Int32 undefined2;
    }
}