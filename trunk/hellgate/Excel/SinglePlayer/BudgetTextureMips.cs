using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetTextureMips
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        public Group group;
        public float diffuse;
        public float normal;
        public float selfIllum;
        public float diffuse2;
        public float specular;
        public float envMap;
        public float lightMap;

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
