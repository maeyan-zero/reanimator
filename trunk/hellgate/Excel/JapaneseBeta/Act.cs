using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ActBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
        [ExcelFile.OutputAttribute(IsBitmask = true, DefaultBitmask = 0)]
        public BitMask01 bitmask01;
		public Int32 minimumExperienceLevelToEnter;
		public float playerVsMonsterIncrementPct;
		public Int32 monsterVsPlayerIncrementPct;
		public Int32 experienceTotal;
        [ExcelOutput(IsTableIndex = true, TableStringId = "STATES")]
        public Int32 requiredState;

        [FlagsAttribute]
        public enum BitMask01 : uint
        {
            betaAccountCanPlay = 1,
            nonSubScriberAccountCanPlay = 2,
			trialAccountCanPLay = 4
        }
    }
}
