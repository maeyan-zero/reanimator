using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Procs : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ProcsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public String name;

            public Int32 code;
            public Int32 verticalCenter;//bool
            public float coolDownInSeconds;
            public Int32 targetInstrumentOwner;//a single bit
            public float delayeProcTimeInSeconds;
            public Int32 skill1;//idx
            public Int32 skill1Param;//idx
            public Int32 skill2;//idx
            public Int32 skill2Param;//idx
        }

        public Procs(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ProcsTable>(data, ref offset, Count);
        }
    }
}
