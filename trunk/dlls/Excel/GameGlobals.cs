using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class GameGlobals : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GameGlobalsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;
            [ExcelTables.ExcelOutput(IsStringOffset=true)]
            public Int32 stringIdOffset;
            Int32 buffer;
            public Int32 unknownInt;
            public float unknownFloat;
        }

        public GameGlobals(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GameGlobalsTable>(data, ref offset, Count);
        }
    }
}
