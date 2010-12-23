using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class QuestCast
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 unitType0;//idx
        public Int32 monster0;//idx
        public Int32 unitType1;//idx
        public Int32 monster1;//idx
        public Int32 unitType2;//idx
        public Int32 monster2;//idx
        public Int32 unitType3;//idx
        public Int32 monster3;//idx
        public Int32 unitType4;//idx
        public Int32 monster4;//idx
        public Int32 unitType5;//idx
        public Int32 monster5;//idx
        public Int32 unitType6;//idx
        public Int32 monster6;//idx
        public Int32 unitType7;//idx
        public Int32 monster7;//idx
    }
}
