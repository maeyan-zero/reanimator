using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LevelsRules
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string drlgFileName;
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_FILE_PATHS")]
        public Int32 folderCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string drlgRuleSet;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string label;
        public Int32 minSubs;
        public Int32 minSubsNightMare;
        public Int32 maxSubs;
        public Int32 maxSubsNightMare;
        public Int32 attempts;
        [ExcelOutput(IsBool = true)]
        public Int32 replaceAll;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 replaceAndCheck;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 onceForReplacement;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 mustPlace;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 weighted;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 exitRule;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 deadEndRule;//bool
        public Int32 looping;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string loopLabel;
        [ExcelOutput(IsBool = true)]
        public Int32 askQuests;//bool
        public Int32 randomQuestPercent;
        [ExcelOutput(IsTableIndex = true, TableStringId = "QUESTS_TASKS_FOR_TUGBOAT")]
        public Int32 questTaskComplete;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme_RunRule;//idx
        [ExcelOutput(IsTableIndex = true, TableStringId = "LEVEL_THEMES")]
        public Int32 theme_SkipRule;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined2;
    }
}
