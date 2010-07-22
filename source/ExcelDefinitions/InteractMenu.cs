using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InteractMenuRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 interaction;//index
        public Int32 stringTitle;//stridx
        public Int32 stringToolTip;//stridx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string frameIcon;
        public Int32 menuButton;

    }
}