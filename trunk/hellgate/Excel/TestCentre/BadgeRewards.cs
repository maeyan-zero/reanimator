using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 badgeName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;//idx
        public Int32 dontApplyIfPlayerHasRewardItemFor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx
        public Int32 minUnitVersion_tcv4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeLimiter_tcv4;
        public Int32 unitTypeLimitPerPlayer_tcv4;
        public Int32 forceOnRespec_tcv4;
        [ExcelOutput(IsScript = true)]
        public Int32 filter_tcv4;
    }
}