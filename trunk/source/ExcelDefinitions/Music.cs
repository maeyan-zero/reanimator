using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 undefined1;
        public Int32 baseCondition;//idx
        public Int32 musicRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined2;
    }
}