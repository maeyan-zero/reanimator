using System;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class GameGlobals : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GameGlobalsTable
        {
            TableHeader header;

            [ExcelOutput(IsStringOffset = true)]
            public Int32 name;
            Int32 _buffer;
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
