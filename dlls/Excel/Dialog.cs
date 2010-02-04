using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class Dialog : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class DialogTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 stringAll;//stridx
            public Int32 stringHellGate;//stridx
            public Int32 stringMythos;//stridx
            public Int32 sound;//idx
            public Int32 mode;//idx
            public Int32 movieListOnFinished;//idx

        }

        public Dialog(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<DialogTable>(data, ref offset, Count);
        }
    }
}
