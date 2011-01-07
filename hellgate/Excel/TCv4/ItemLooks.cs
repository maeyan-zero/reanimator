using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooksTCv4
    {
        ExcelFile.RowHeader header;


        [ExcelOutput(SortColumnOrder = 1, SecondarySortColumn = "lookGroup", IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEM_LOOK_GROUPS")]
        public Int32 lookGroup;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string folder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WARDROBE_LAYER")]
        public Int32 wardrobe;//idx;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string iconTexture_tcv4;
        public Int32 undefined2;
        public Int32 undefined_tcv4;
    }
}