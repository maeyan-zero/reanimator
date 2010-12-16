using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InteractMenu
    {
        TableHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 interaction;//index
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringTitle;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringToolTip;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string frameIcon;
        public Int32 menuButton;
    }
}
