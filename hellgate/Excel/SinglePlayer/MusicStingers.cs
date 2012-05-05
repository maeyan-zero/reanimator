using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicStingers
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//first entry in index is empty.
        public Type type;
        public Int32 fadeOutBeats;
        public Int32 fadeInBeats;
        public Int32 fadeInDelayBeats;
        public Int32 fadeOutDelayBeats;
        public Int32 introBeats;
        public Int32 outroBeats;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDS")]
        public Int32 soundGroup;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] undefined;

        public enum Type
        {
            Null = -1,
            Layer = 0,
            Interrupt_Breakdown = 1,
            Override = 2
        }
    }
}