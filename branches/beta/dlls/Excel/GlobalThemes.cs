using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class GlobalThemes : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GlobalThemesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 startMonth;
            public Int32 startDay;
            public Int32 startDayOfWeek;
            public Int32 endMonth;
            public Int32 endDay;
            public Int32 endDayOfWeek;
            public Int32 treasureClassPreAndPost0a;
            public Int32 treasureClassPreAndPost0b;
            public Int32 treasureClassPreAndPost1a;
            public Int32 treasureClassPreAndPost1b;
            public Int32 treasureClassPreAndPost2a;
            public Int32 treasureClassPreAndPost2b;
            public Int32 treasureClassPreAndPost3a;
            public Int32 treasureClassPreAndPost3b;
            public Int32 treasureClassPreAndPost4a;
            public Int32 treasureClassPreAndPost4b;
            public Int32 treasureClassPreAndPost5a;
            public Int32 treasureClassPreAndPost5b;
            public Int32 treasureClassPreAndPost6a;
            public Int32 treasureClassPreAndPost6b;
            public Int32 treasureClassPreAndPost7a;
            public Int32 treasureClassPreAndPost7b;
            public Int32 activateByTime;
        }

        public GlobalThemes(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GlobalThemesTable>(data, ref offset, Count);
        }
    }
}
