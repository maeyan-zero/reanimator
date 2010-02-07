using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Weather : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WeatherTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 state;//idx
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] themes;
        }

        public Weather(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WeatherTable>(data, ref offset, Count);
        }
    }
}
