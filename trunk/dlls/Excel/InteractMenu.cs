using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class InteractMenu : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class InteractMenuTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 interaction;//index
            public Int32 stringTitle;//stridx
            public Int32 stringToolTip;//stridx
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string frameIcon;
            public Int32 menuButton;

        }

        public InteractMenu(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<InteractMenuTable>(data, ref offset, Count);
        }
    }
}