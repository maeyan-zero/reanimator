using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;


namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillGroupsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(SortAscendingID = 2)]
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