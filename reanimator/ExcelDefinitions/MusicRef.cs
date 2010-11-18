using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicRefRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 undefined1;
        public float undefined2;
        public float undefined3;
        public Int32 beatsPerMeasure;
        public Int32 offsetInMilliSeconds;
        public Int32 grooveUpdateInMeasures;
        public Int32 defaultGroove;//idx
        public Int32 offGoove;//idx
        public Int32 undefined4;
        public Int32 soundGroup;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        Int32[] undefined;
    }
}