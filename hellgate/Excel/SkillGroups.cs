using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillGroups
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 displayString;//stridx
        [ExcelOutput(IsBool = true)]
        public Int32 displayInSkillString;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 dontClearCooldownOnDeath;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string backGroundIcon;
    }
}
