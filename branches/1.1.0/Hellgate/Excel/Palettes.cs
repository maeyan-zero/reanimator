using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PalettesRow
    {
        ExcelFile.TableHeader header;

        public Int32 unknown1;
        public Int32 unknown2;
        public Int32 unknown3;
        public Int32 unknown4;
        public Int32 unknown5;
        public Int32 unknown6;
    }
}