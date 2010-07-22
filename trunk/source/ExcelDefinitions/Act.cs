using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ActRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        public Int32 bitmask;/*0 bit beta account can play
	1 bit non_subscriber account can play*/
    }
}
