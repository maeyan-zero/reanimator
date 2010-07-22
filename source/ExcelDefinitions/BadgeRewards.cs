using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;

        public Int32 code;
        public Int32 badgeName;
        public Int32 item;//idx
        public Int32 dontApplyIfPlayerHasRewardItemF;//idx, and yes, it finishes just like that.
        public Int32 state;//idx
    }
}