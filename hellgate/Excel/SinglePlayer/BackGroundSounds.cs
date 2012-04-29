using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS2D")]
        public Int32 sounds2D9;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D8;
        [ExcelOutput(IsTableIndex = true, TableStringId = "BACKGROUNDSOUNDS3D")]
        public Int32 sounds3D9;
    }
}
