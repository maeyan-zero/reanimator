using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FatigueBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 penaltyState;
        public Int32 firstMessageInMinutes;
        public Int32 lastMessageInMinutes;
        public Int32 messageRepeatTimeInMinutes;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 message;
    }
}
