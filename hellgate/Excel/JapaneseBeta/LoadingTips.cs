using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LoadingTipsBeta
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringKey;//stridx
        public Int32 weight;
        [ExcelOutput(IsScript = true)]
        public Int32 condition;//intptr
        [ExcelOutput(IsBool = true)]
        public Int32 dontUseWithoutAGame;
        [ExcelOutput(IsBool = true)]
        public Int32 isPvpToolTip;
    }
}
