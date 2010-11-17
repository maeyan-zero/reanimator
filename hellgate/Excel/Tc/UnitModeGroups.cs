using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel.Tc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class UnitModeGroupsTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] undefined;
        public Int32 undefined_tcv4;
    }
}