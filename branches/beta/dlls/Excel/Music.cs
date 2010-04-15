using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Music : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MusicTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 undefined1;
            public Int32 baseCondition;//idx
            public Int32 musicRef;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined2;
        }

        public Music(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MusicTable>(data, ref offset, Count);
        }
    }
}
