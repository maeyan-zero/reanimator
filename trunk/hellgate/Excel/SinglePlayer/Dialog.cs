using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Dialog
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringAll;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringHellGate;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringMythos;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 sound;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITMODES")]
        public Int32 mode;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MOVIELISTS")]
        public Int32 movieListOnFinished;//idx
    }
}
