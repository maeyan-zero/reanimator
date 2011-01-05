using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WeatherSets
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather1;//idx
        public Int32 weight1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather2;//idx
        public Int32 weight2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather3;//idx
        public Int32 weight3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather4;//idx
        public Int32 weight4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather5;//idx
        public Int32 weight5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather6;//idx
        public Int32 weight6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather7;//idx
        public Int32 weight7;
        [ExcelOutput(IsTableIndex = true, TableStringId = "WEATHER")]
        public Int32 weather8;//idx
        public Int32 weight8;
    }
}