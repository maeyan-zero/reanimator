using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        public Int32 badgeName;
        public Int32 item;//idx
        public Int32 dontApplyIfPlayerHasRewardItemF;//idx, and yes, it finishes just like that.
        public Int32 state;//idx
        public Int32 minUnitVersion_tcv4;
        public Int32 unitTypeLimiter_tcv4;
        public Int32 unitTypeLimitPerPlayer_tcv4;
        public Int32 forceOnRespec_tcv4;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 filter_tcv4;
    }
}