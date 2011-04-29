using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ActionBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        public Int32 unknown1;
        public Int32 unknown2;
        public Int32 unknown3;
        public Int32 unknown4;
        public Int32 unknown5;
        public Int32 unknown6;
        public Int32 unknown7;
    }
}
