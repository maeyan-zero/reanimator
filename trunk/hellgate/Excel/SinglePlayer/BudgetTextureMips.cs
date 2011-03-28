using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetTextureMips
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        public Int32 group;
        public float diffuse;
        public float normal;
        public float selfIllum;
        public float diffuse2;
        public float specular;
        public float envMap;
        public float lightMap;
    }
}
