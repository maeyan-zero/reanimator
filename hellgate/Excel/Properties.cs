using System;
using System.Runtime.InteropServices;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Properties
    {
        TableHeader header;
        public Int32 property;
    }
}
