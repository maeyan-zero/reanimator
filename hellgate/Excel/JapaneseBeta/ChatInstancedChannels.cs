using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChatInstancedChannelsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
        public string name;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 optOut;
        public Int32 maxMembers;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 code;
    }
}
