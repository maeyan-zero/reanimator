using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Bones
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
    }
}