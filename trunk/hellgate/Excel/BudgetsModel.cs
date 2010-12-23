using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetsModel
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        public Int32 group;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public float lodRate;
    }
}