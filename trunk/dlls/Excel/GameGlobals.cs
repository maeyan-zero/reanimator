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
            TableHeader header;

            [ExcelTables.ExcelOutput(IsStringOffset=true)]
            public Int32 name;
            Int32 buffer;
            public Int32 intValue;
            public float floatValue;
        }

        public GameGlobals(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<GameGlobalsTable>(data, ref offset, Count);
        }
    }
}
