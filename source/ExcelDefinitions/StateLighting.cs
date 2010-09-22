using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class StateLightingRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string sh_cubemap_filename;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 312)]
        byte[] unknown1;
    }
}