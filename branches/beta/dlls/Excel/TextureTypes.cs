using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class TextureTypes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class TextureTypesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 backgroundPriority;
            public Int32 unitPriority;
            public Int32 particlePriority;
            public Int32 uiPriority;
            public Int32 wardrobePriority;
        }

        public TextureTypes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<TextureTypesTable>(data, ref offset, Count);
        }
    }
}
