using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LoadingTipsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 stringKey;//stridx
        public Int32 weight;
        public Int32 condition;//intptr
        public Int32 dontUseWithoutAGame;
    }
}