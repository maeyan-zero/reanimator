using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobePartRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string label;
        public Int32 partGroup;
        public Int32 materialIndex;
        public Int32 targetTexture;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string materialName;
    }
}