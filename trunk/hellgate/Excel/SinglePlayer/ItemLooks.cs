using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooks
    {
        RowHeader header;
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
        public Int32 undefined2;
    }
}
