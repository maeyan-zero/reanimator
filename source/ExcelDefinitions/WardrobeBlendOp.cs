using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBlendOpRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 replaceAllParts;//bool;
        public Int32 noTextureChange;//bool;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] removeParts;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] addParts;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string blend;
        public short undefined1;
        public Int32 undefined2;
        public Int32 target;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] covers;
    }
}