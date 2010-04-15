using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Difficulty : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class DifficultyTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String name;

            public Int32 code;
            public Int32 unlockedString;//stridx
        }

        public Difficulty(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<DifficultyTable>(data, ref offset, Count);
        }
    }
}
