using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RegionRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionLong;
        public Int32 isDefault;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string regionShort;
        public Int32 code;
    }
}