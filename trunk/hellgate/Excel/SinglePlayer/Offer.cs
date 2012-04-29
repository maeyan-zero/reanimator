using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Offer
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 noDuplicates;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 doNotIdentify;//bool
        public Int32 numAllowedTakes;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure0UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure0;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure1UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure1;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure2UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure2;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure3UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure3;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure4UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure4;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure5UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure5;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure6UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure6;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 treasure7UnitType;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure7;//idx
        [ExcelOutput(IsStringIndex = true)]
        public Int32 offerString;//stridx
    }
}
