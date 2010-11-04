using System;
using System.Runtime.InteropServices;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Palettes
    {
        TableHeader header;
        public Int32 unknown1;
        public Int32 unknown2;
        public Int32 unknown3;
        public Int32 unknown4;
        public Int32 unknown5;
        public Int32 unknown6;
    }
}
