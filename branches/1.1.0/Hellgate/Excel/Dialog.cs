using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Dialog
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 stringAll;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 stringHellGate;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 stringMythos;
        public Int32 sound;//idx
        public Int32 mode;//idx
        public Int32 movieListOnFinished;//idx
    }
}