using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundMixStatesTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 priority;
        public Int32 values1;//used
        public Int32 values2;//used
        public Int32 values3;//used
        public Int32 values4;//used
        public Int32 values5;//used
        public Int32 values6;//used
        public Int32 values7;//used
        public Int32 values8;
        public Int32 values9_tcv4;
        public Int32 values10_tcv4;
        public Int32 values11_tcv4;
        public Int32 values12_tcv4;
        public Int32 values13_tcv4;
        public Int32 values14_tcv4;
        public Int32 values15_tcv4;
        public Int32 values16_tcv4;
        public Int32 sets1_tcv4;//used
        public Int32 sets2_tcv4;//used
        public Int32 sets3_tcv4;//used
        public Int32 sets4_tcv4;
        public Int32 sets5_tcv4;
        public Int32 sets6_tcv4;
        public Int32 sets7_tcv4;
        public Int32 sets8_tcv4;
        public Int32 sets9_tcv4;
        public Int32 sets10_tcv4;
        public Int32 sets11_tcv4;
        public Int32 sets12_tcv4;
        public Int32 sets13_tcv4;
        public Int32 sets14_tcv4;
        public Int32 sets15_tcv4;
        public Int32 sets16_tcv4;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
        Int32[] undefined_tcv4;
        public float fadeInTimeInSeconds;
        public Int32 undefined1;
        public float fadeOutTimeInSeconds;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string reverbOverRide;
        public Int32 undefined3;
        public Int32 undefined4;
    }
}