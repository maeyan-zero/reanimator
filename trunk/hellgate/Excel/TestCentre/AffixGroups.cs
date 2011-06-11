using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel.TestCentre
{
    /* Total Size = 88 Bytes
     * 
     * 03 00 00 00 3F 00 00 00 00 00 FF FF 00 00 FF FF
     * 
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 01 00 00 00
     * 03 00 00 00
     * FF FF FF FF
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     * 00 00 00 00
     */

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class AffixGroupsTCv4
    {
        RowHeader header;

        Int32 _internal00;
        Int32 _internal04;
        [ExcelOutput(IsStringOffset = true, SortColumnOrder = 1)]
        public Int32 name;
        Int32 null0C;
        public Int32 weight;
        [ExcelOutput(IsTableIndex = true, TableStringId = "UNITTYPES")]
        public Int32 onlyUnitType;
        [ExcelOutput(IsTableIndex = true, TableStringId = "AFFIX_GROUPS")]
        public Int32 parent;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        Int32[] _internal1C;
    }
}