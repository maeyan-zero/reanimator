using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AchievementSlotsBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String slotName;
        [ExcelFile.OutputAttribute(SortColumnOrder = 2)]
        public Int32 code;
		public Int32 unlocksAtLevel;
        [ExcelFile.OutputAttribute(IsScript = true)]
        public Int32 conditionalScript;
        [ExcelFile.OutputAttribute(IsBool = true)]
		public Int32 unlocksAtPCBang;
		
    }
}
