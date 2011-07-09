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
        public Group group;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public float lodRate;

        public enum Group
        {
            Null = -1,
            Background = 0,
            Units = 1,
            Particle = 2,
            UI = 3,
            Wardrobe = 4
        }
    }
}