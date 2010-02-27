using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SoundVidCas : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SoundVidCasTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 unknown1;
        }

        public SoundVidCas(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SoundVidCasTable>(data, ref offset, Count);
        }
    }
}
