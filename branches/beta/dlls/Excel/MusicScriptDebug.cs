using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MusicScriptDebug : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicScriptDebugTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 script;//intptr
        }

        public MusicScriptDebug(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicScriptDebugTable>(data, ref offset, Count);
        }
    }
}
