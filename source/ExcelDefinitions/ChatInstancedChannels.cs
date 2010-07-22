using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ChatInstancedChannelsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
        public string Channel;
        public Int32 unknown1;
        public Int32 unknown2;
        public Int32 unknown3;
    }
}