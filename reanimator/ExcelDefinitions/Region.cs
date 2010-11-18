using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RegionRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionLong;
        public Int32 isDefault;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionShort;
        public Int32 code;
    }
}