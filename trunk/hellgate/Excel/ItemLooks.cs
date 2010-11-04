using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooks
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
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
