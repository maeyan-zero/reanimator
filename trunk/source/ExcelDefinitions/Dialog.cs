using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DialogRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 stringAll;//stridx
        public Int32 stringHellGate;//stridx
        public Int32 stringMythos;//stridx
        public Int32 sound;//idx
        public Int32 mode;//idx
        public Int32 movieListOnFinished;//idx

    }
}