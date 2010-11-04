using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetsModel
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        public Int32 group;
        public float lodRate;
    }
}
