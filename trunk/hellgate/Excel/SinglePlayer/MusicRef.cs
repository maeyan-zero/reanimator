using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicRef
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 undefined1;
        public float undefined2;
        public float undefined3;
        public Int32 beatsPerMeasure;
        public Int32 offsetInMilliSeconds;
        public Int32 grooveUpdateInMeasures;
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 defaultGroove;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "MUSICGROOVELEVELS")]
        public Int32 offGoove;//idx
        public Int32 undefined4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 soundGroup;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        Int32[] undefined;
    }
}
