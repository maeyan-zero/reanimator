using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        public Int32 sounds2D0;
        public Int32 sounds2D1;
        public Int32 sounds2D2;
        public Int32 sounds2D3;
        public Int32 sounds2D4;
        public Int32 sounds2D5;
        public Int32 sounds2D6;
        public Int32 sounds2D7;
        public Int32 sounds2D8;
        public Int32 sounds2D9;
        public Int32 sounds3D0;
        public Int32 sounds3D1;
        public Int32 sounds3D2;
        public Int32 sounds3D3;
        public Int32 sounds3D4;
        public Int32 sounds3D5;
        public Int32 sounds3D6;
        public Int32 sounds3D7;
        public Int32 sounds3D8;
        public Int32 sounds3D9;
    }
}
