using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class StateLightingRow
    {
        ExcelFile.TableHeader header;

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