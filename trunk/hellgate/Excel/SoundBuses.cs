using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SoundBuses
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 volume;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 undefined;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string effects;
        [ExcelOutput(IsTableIndex = true, TableStringId = "SOUNDBUSES")]
        public Int32 sendsTo;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca1;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca2;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca3;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca4;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca5;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca6;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca7;//idx
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 vca8;//idx
        public Int32 userControl;
    }
}