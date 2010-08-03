using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class DialogRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsStringId = true, Table = "Strings_Quest")]
        public Int32 stringAll;
        [ExcelOutput(IsStringId = true, Table = "Strings_Quest")]
        public Int32 stringHellGate;
        [ExcelOutput(IsStringId = true, Table = "Strings_Quest")]
        public Int32 stringMythos;
        public Int32 sound;//idx
        public Int32 mode;//idx
        public Int32 movieListOnFinished;//idx
    }
}