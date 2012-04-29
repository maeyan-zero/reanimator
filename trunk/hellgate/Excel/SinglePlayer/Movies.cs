using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
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
        public Language audioLanguages0;
        public Language audioLanguages1;
        public Language audioLanguages2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] audioLanguages;
        public RegionList regionList0;
        public RegionList regionList1;
        public RegionList regionList2;
        public RegionList regionList3;
        public RegionList regionList4;
        public RegionList regionList5;
        public RegionList regionList6;
        public RegionList regionList7;
        public RegionList regionList8;
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
        public Int32 disallowInCensoredSku;//bool
        public float creditMovieDisplayedInSeconds;

        public enum Language
        {
            Null = -1,
            English = 0,
            Korean = 1,
            ChineseSimplified = 2,
            ChineseTraditional = 3,
            Japanese = 4,
            French = 5,
            Spanish = 6,
            German = 7,
            Italian = 8,
            Polish = 9,
            Czech = 10,
            Hungarian = 11,
            Russian = 12,
            Thai = 13,
            Vietnamese = 14
        }
        public enum RegionList
        {
            Null = -1,
            NorthAmerica = 0,
            Europe = 1,
            Japan = 2,
            Korea = 3,
            China = 4,
            Taiwan = 5,
            SouthEastAsia = 6,
            SouthAmerica = 7,
            Australia = 8
        }
    }
}
