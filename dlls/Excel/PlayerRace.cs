using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class PlayerRace : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class PlayerRaceTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 String;//stridx
            public Int32 code;
        }

        public PlayerRace(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<PlayerRaceTable>(data, ref offset, Count);
        }
    }
}
