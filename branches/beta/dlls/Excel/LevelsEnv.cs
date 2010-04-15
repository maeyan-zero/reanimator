using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LevelsEnv : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsEnvTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 undefined;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string fileName;
        }

        public LevelsEnv(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsEnvTable>(data, ref offset, Count);
        }
    }
}
