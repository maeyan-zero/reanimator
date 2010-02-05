using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class UIComponent : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class UIComponentTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string script;
        }

        public UIComponent(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<UIComponentTable>(data, ref offset, Count);
        }
    }
}