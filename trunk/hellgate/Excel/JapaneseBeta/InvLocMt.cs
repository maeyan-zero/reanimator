using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InvLocMtBeta
    {
        ExcelFile.RowHeader header;
        public Int32 lType;
        public Int32 mType;
        public Int32 sType;
        //[ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "")]//table 18
        public Int32 location;
    }
}
