using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Bones : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BonesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
        }

        public Bones(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BonesTable>(data, ref offset, Count);
        }
    }
}
