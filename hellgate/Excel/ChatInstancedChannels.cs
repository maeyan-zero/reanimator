using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChatInstancedChannels
    {
        RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
        public string Channel;
        public Int32 unknown1;
        public Int32 unknown2;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 unknown3;
    }
}
