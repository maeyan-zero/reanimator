using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChatInstancedChannel
    {
        TableHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
        public String channel;
        public Int32 unknown1;
        public Int32 unknown2;
        [ExcelAttribute(IsIntOffset = true)]
        public Int32 unknown3;
    }
}