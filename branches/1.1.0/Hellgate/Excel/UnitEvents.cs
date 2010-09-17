using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitEventsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        byte[] unknown2;
        public Int32 recordPlayer;
        public Int32 recordMonster;
        public Int32 recordMissile;
        public Int32 recordItem;
        public Int32 recordObject;
    }
}