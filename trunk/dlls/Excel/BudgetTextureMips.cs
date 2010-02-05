using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BudgetTextureMips : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BudgetTextureMipsTable
        {
            TableHeader header;

            public Int32 group;
            public float diffuse;
            public float normal;
            public float selfIllum;
            public float diffuse2;
            public float specular;
            public float envMap;
            public float lightMap;

        }

        public BudgetTextureMips(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BudgetTextureMipsTable>(data, ref offset, Count);
        }
    }
}
