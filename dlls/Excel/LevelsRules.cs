using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LevelsRules : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LevelsRulesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string drlgFileName;
            public Int32 undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string drlgRuleSet;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string label;

            public Int32 minSubs;
            public Int32 minSubsNightMare;
            public Int32 maxSubs;
            public Int32 maxSubsNightMare;
            public Int32 attempts;
            public Int32 replaceAll;//bool
            public Int32 replaceAndCheck;//bool
            public Int32 onceForReplacement;//bool
            public Int32 mustPlace;//bool
            public Int32 weighted;//bool
            public Int32 exitRule;//bool
            public Int32 deadEndRule;//bool
            public Int32 looping;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string loopLabel;
            public Int32 askQuests;//bool
            public Int32 randomQuestPercent;
            public Int32 questTaskComplete;//idx
            public Int32 theme_RunRule;//idx
            public Int32 theme_SkipRule;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            Int32[] undefined2;

        }

        public LevelsRules(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LevelsRulesTable>(data, ref offset, Count);
        }
    }
}
