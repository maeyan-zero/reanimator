using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class SkillGroups : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class SkillGroupsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 displayString;//stridx
            public Int32 displayInSkillString;//bool
            public Int32 dontClearCooldownOnDeath;//bool
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string backGroundIcon;
        }

        public SkillGroups(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<SkillGroupsTable>(data, ref offset, Count);
        }
    }
}
