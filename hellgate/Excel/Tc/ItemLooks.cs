using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel.Tc
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ItemLooksTCv4Row
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        public Int32 item;//idx
        public Int32 lookGroup;//idx
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string folder;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;
        public Int32 wardrobe;//idx;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string iconTexture_tcv4;
        public Int32 undefined2;
        public Int32 undefined_tcv4;
    }
}