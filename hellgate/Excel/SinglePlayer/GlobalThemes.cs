using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class GlobalThemes
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Month startMonth;
        public Int32 startDay;
        public DayOfWeek startDayOfWeek;
        public Month endMonth;
        public Int32 endDay;
        public DayOfWeek endDayOfWeek;
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

        public enum Month
        {
            Null = -1,
            ChooseAgain = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum DayOfWeek
        {
            Null = -1,
            Sunday = 0,
            Monday = 1,
            Tuesday = 2,
            WednesDay = 3,
            Thursday = 4,
            Friday = 5,
            Saturday = 6
        }
    }
}
