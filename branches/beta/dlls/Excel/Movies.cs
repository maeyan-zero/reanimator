using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Movies : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MoviesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string fileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string lowResFileName;

            public Int32 audioLanguages0;
            public Int32 audioLanguages1;
            public Int32 audioLanguages2;
            public Int32 audioLanguages3;
            public Int32 audioLanguages4;
            public Int32 audioLanguages5;
            public Int32 audioLanguages6;
            public Int32 audioLanguages7;
            public Int32 audioLanguages8;
            public Int32 audioLanguages9;
            public Int32 regionList0;
            public Int32 regionList1;
            public Int32 regionList2;
            public Int32 regionList3;
            public Int32 regionList4;
            public Int32 regionList5;
            public Int32 regionList6;
            public Int32 regionList7;
            public Int32 regionList8;
            public Int32 regionList9;
            public Int32 regionList10;
            public Int32 regionList11;
            public Int32 regionList12;
            public Int32 regionList13;
            public Int32 regionList14;
            public Int32 regionList15;
            public Int32 regionList16;
            public Int32 regionList17;
            public Int32 regionList18;
            public Int32 regionList19;
            public Int32 regionList20;
            public Int32 regionList21;
            public Int32 regionList22;
            public Int32 regionList23;
            public Int32 regionList24;
            public Int32 regionList25;
            public Int32 regionList26;
            public Int32 regionList27;
            public Int32 regionList28;
            public Int32 regionList29;
            public Int32 regionList30;
            public Int32 regionList31;
            public Int32 loops;//bool
            public Int32 useInCredits;//bool
            public Int32 wideScreenOnly;//bool
            public Int32 canPause;//bool
            public Int32 noSkip;//bool
            public Int32 noSkipFirstTime;//bool
            public Int32 putInMainPakFile;//bool
            public Int32 forceAllowHighQuality;//bool
            public Int32 onlyWithCensoredSku;//bool
            public Int32 disAllowInCensoredSku;//bool
            public float creditMovieDisplayedInSeconds;
        }

        public Movies(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MoviesTable>(data, ref offset, Count);
        }
    }
}
