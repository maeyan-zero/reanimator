using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class BadgeRewards : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class BadgeRewardsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string badgeRewardName;

            public Int32 code;
            public Int32 badgeName;
            public Int32 item;//idx
            public Int32 dontApplyIfPlayerHasRewardItemF;//idx, and yes, it finishes just like that.
            public Int32 state;//idx
        }

        public BadgeRewards(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<BadgeRewardsTable>(data, ref offset, Count);
        }
    }
}
