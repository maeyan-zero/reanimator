using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel.Tc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SkillTabsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        public Int32 displayString;//stridx
        [ExcelOutput(IsBool = true)]
        public Int32 drawOnlyKnown;//bool
        public Int32 perkTab_tcv4;//bool
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string iconTextureName;
    }
}
