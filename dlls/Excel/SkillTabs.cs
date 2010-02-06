using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SkillTabs : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SkillTabsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 displayString;//stridx
            public Int32 drawOnlyKnown;//bool
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string iconTextureName;
        }

        public SkillTabs(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SkillTabsTable>(data, ref offset, Count);
        }
    }
}
