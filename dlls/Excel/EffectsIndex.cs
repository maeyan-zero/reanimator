using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class EffectsIndex : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class EffectsIndexTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string name;

            public Int32 fixedFunc;//idx
            public Int32 sm_11;//idx
            public Int32 sm_20_Low;//idx
            public Int32 sm_20_High;//idx
            public Int32 sm_30;//idx
            public Int32 sm_40;//idx
            public Int32 required;//bool
        }

        public EffectsIndex(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<EffectsIndexTable>(data, ref offset, Count);
        }
    }
}
