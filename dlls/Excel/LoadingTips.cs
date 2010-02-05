using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class LoadingTips : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class LoadingTipsTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 stringKey;//stridx
            public Int32 weight;
            public Int32 condition;//intptr
            public Int32 dontUseWithoutAGame;
        }

        public LoadingTips(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<LoadingTipsTable>(data, ref offset, Count);
        }
    }
}
