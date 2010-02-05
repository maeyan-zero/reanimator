using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MaterialsGlobal : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MaterialsGlobalTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string material;
            public Int32 undefined;
        }

        public MaterialsGlobal(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MaterialsGlobalTable>(data, ref offset, Count);
        }
    }
}
