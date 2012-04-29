using System;
using System.Runtime.InteropServices;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Palettes
    {
        RowHeader header;
        public Int32 unknown1;
        public Int32 unknown2;
        public Int32 unknown3;
        public Int32 unknown4;
        public Int32 unknown5;
        public Int32 unknown6;
    }
}
