using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class LoadingTipsRow
    {
        ExcelFile.TableHeader header;
         [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 stringKey;//stridx
        public Int32 weight;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 condition;//intptr
        public Int32 dontUseWithoutAGame;
    }
}