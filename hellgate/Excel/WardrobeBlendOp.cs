using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeBlendOp
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsBool = true)]
        public Int32 replaceAllParts;//bool;
        [ExcelOutput(IsBool = true)]
        public Int32 noTextureChange;//bool;
        public Int32 removeParts1;
        public Int32 removeParts2;
        public Int32 removeParts3;
        public Int32 removeParts4;
        public Int32 removeParts5;
        public Int32 removeParts6;
        public Int32 removeParts7;
        public Int32 removeParts8;
        public Int32 removeParts9;
        public Int32 removeParts10;
        public Int32 addParts1;
        public Int32 addParts2;
        public Int32 addParts3;
        public Int32 addParts4;
        public Int32 addParts5;
        public Int32 addParts6;
        public Int32 addParts7;
        public Int32 addParts8;
        public Int32 addParts9;
        public Int32 addParts10;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string blend;
        short undefined1;
        [ExcelOutput(ConstantValue = -1)]
        Int32 undefined2;
        public Int32 target;
        public Int32 covers1;
        public Int32 covers2;
        public Int32 covers3;
        public Int32 covers4;
        public Int32 covers5;
        public Int32 covers6;
        public Int32 covers7;
        public Int32 covers8;
        public Int32 covers9;
        public Int32 covers10;
    }
}