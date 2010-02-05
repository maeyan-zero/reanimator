using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BudgetsModel : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BudgetsModelTable
        {
            TableHeader header;

            public Int32 group;
            public float lodRate;
        }

        public BudgetsModel(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BudgetsModelTable>(data, ref offset, Count);
        }
    }
}
