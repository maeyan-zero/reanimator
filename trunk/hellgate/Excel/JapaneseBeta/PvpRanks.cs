using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PvpRanksBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string rankName;
        public Int32 code;
        public Int32 pvpExpMin;
        public Int32 maxPercentile;
        public Int32 maxRankPlayer;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 minusExpEnable;//bool
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 characterSheetString;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string iconTextureName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string characterSheetIcon;
    }
}
