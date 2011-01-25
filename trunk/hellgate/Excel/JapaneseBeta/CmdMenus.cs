using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class CmdMenusBeta
    {
        ExcelFile.RowHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string cmdMenu;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string chatCommand;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 immediate;
    }
}
