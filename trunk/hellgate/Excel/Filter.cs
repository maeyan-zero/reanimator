using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Filter
    {
        TableHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        public Int32 unknown1;
        public Int32 unknown2;
    }
}
