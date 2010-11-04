using System;
using System.Runtime.InteropServices;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class PlayerRace
    {
        TableHeader header;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 String;//stridx
        public Int32 code;
    }
}
