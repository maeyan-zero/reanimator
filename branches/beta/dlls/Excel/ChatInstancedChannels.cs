using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ChatInstancedChannels : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ChatInstancedChannelsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
            public string Channel;
            public Int32 unknown1;
            public Int32 unknown2;
            public Int32 unknown3;
        }

        public ChatInstancedChannels(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ChatInstancedChannelsTable>(data, ref offset, Count);
        }
    }
}
