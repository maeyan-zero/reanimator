using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class FootSteps
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 concrete;//idx
        public Int32 wood;//idx
        public Int32 metal;//idx
        public Int32 tile;//idx
        public Int32 squishy;//idx
        public Int32 gravel;//idx
        public Int32 snow;//idx
        public Int32 dirt;//idx
        public Int32 water;//idx
        public Int32 rubble;//idx
        public Int32 undefined;//idx
    }
}