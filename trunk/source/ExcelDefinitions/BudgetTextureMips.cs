using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BudgetTextureMipsRow
    {
        ExcelFile.TableHeader header;

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