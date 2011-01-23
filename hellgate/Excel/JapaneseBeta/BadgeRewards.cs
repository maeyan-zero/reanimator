using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 badgeName;//count1	type24
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;//idx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table B1
        public Int32 dontApplyIfPlayerHasRewardItemFor;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx
		public Int32 minUnitVersion;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeLimiter;//idx
		public Int32 unitTypeLimitPerPlayer;
        [ExcelOutput(IsBool = true)]
		public Int32 forceOnRespec;
        [ExcelOutput(IsScript = true)]
		public Int32 filter;
        [ExcelOutput(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure;//idx
		
    }
}
