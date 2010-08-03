using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundBusesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 volume;
        public Int32 undefined;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        public Int32 sendsTo;//idx
        public Int32 vca1;
        public Int32 vca2;
        public Int32 vca3;
        public Int32 vca4;
        public Int32 vca5;
        public Int32 vca6;
        public Int32 vca7;
        public Int32 vca8;
        public Int32 userControl;
    }
}