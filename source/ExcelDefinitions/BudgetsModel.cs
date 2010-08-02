using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetsModelRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        public Int32 group;
        public float lodRate;
    }
}