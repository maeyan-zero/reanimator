using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Movies
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string fileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string lowResFileName;
        public Int32 audioLanguages0;
        public Int32 audioLanguages1;
        public Int32 audioLanguages2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] audioLanguages;
        public Int32 regionList0;
        public Int32 regionList1;
        public Int32 regionList2;
        public Int32 regionList3;
        public Int32 regionList4;
        public Int32 regionList5;
        public Int32 regionList6;
        public Int32 regionList7;
        public Int32 regionList8;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
        Int32[] regionList;
        [ExcelOutput(IsBool = true)]
        public Int32 loops;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 useInCredits;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 wideScreenOnly;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 canPause;//bool
        [ExcelOutput(IsBool = true)]
        Int32 noSkip;//bool // always 0
        [ExcelOutput(IsBool = true)]
        public Int32 noSkipFirstTime;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 putInMainPakFile;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 forceAllowHighQuality;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 onlyWithCensoredSku;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 disAllowInCensoredSku;//bool
        public float creditMovieDisplayedInSeconds;
    }
}
