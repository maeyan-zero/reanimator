using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionStandingRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        public Int32 displayString;//stridx
        public Int32 displayStringNumbers;//stridx
        public Int32 minScore;
        public Int32 maxScore;
        public Int32 mood;
    }
}