using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FactionRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        public Int32 displayString;//stridx
        public Int32 unitTypeStartStanding1;
        public Int32 levelDefStartStanding1;
        public Int32 startStanding1;
        public Int32 unitTypeStartStanding2;
        public Int32 levelDefStartStanding2;
        public Int32 startStanding2;
        public Int32 unitTypeStartStanding3;
        public Int32 levelDefStartStanding3;
        public Int32 startStanding3;
        public Int32 unitTypeStartStanding4;
        public Int32 levelDefStartStanding4;
        public Int32 startStanding4;
    }
}