using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BadgeRewardsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string badgeRewardName;
        [ExcelOutput(SortId = 2)]
        public Int32 code;
        public Int32 badgeName;
        public Int32 item;//idx
        public Int32 dontApplyIfPlayerHasRewardItemF;//idx, and yes, it finishes just like that.
        public Int32 state;//idx
        [ExcelOutput(Exclude = true)]
        public Int32 TCV4_1;
        [ExcelOutput(Exclude = true)]
        public Int32 TCV4_2;
        [ExcelOutput(Exclude = true)]
        public Int32 TCV4_3;
        [ExcelOutput(Exclude = true)]
        public Int32 TCV4_4;
        [ExcelOutput(Exclude = true, IsIntOffset = true)]
        public Int32 TCV4_5;
    }
}