using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 badgeName;//count1	type24
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 item;//idx
        //[ExcelOutput(IsTableIndex = true, TableStringId = "")]//table B1
        public Int32 dontApplyIfPlayerHasRewardItemFor;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 state;//idx
		public Int32 minUnitVersion;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 unitTypeLimiter;//idx
		public Int32 unitTypeLimitPerPlayer;
        [ExcelFile.OutputAttribute(IsBool = true)]
		public Int32 forceOnRespec;
        [ExcelFile.OutputAttribute(IsScript = true)]
		public Int32 filter;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "TREASURE")]
        public Int32 treasure;//idx
		
    }
}
