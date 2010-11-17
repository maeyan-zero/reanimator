using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Dialog
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringAll;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringHellGate;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringMythos;
        public Int32 sound;//idx
        public Int32 mode;//idx
        public Int32 movieListOnFinished;//idx
    }
}
