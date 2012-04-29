using System;
using System.Runtime.InteropServices;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Properties
    {
        RowHeader header;
        public Int32 property;
    }
}
