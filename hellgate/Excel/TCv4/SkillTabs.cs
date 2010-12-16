using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TCv4
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillTabsTCv4
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 displayString;//stridx
        [ExcelOutput(IsBool = true)]
        public Int32 drawOnlyKnown;//bool
        public Int32 perkTab_tcv4;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string iconTextureName;
    }
}
