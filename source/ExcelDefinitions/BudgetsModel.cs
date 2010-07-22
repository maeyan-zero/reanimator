using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetsModelRow
    {
        ExcelFile.TableHeader header;

        public Int32 group;
        public float lodRate;
    }
}