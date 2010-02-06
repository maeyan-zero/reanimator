using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MovieSubTitles : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MovieSubTitlesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 movie0;
            public Int32 movie1;
            public Int32 movie2;
            public Int32 movie3;
            public Int32 language;//idx
            public Int32 String;//stridx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            Int32[] undefined;
        }

        public MovieSubTitles(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MovieSubTitlesTable>(data, ref offset, Count);
        }
    }
}
