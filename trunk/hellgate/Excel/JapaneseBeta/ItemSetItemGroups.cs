using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemSetItemGroupsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 setAffix1NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix1;
        public Int32 setAffix2NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix2;
        public Int32 setAffix3NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix3;
        public Int32 setAffix4NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix4;
        public Int32 setAffix5NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix5;
        public Int32 setAffix6NumRequired;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "AFFIXES")]//table 35
        public Int32 setAffix6;
    }
}
