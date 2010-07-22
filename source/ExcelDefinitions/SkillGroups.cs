using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillGroupsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        public Int32 displayString;//stridx
        public Int32 displayInSkillString;//bool
        public Int32 dontClearCooldownOnDeath;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string backGroundIcon;
    }
}