using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace Reanimator.Excel
{
    public class States : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class StatesTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            public Int32 stringId;
            Int32 buffer;
            public Int32 id;
            public Int32 unknown1;
            public Int32 effect;
            public Int32 unknown3;
            public Int32 unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
            public Int32 unknown7;
            public Int32 unknown8;
            public Int32 unknown9;
            public Int32 unknown10;
            public Int32 unknown11;
            public Int32 unknown12;
            public Int32 unknown13;
            public Int32 unknown14;
            public Int32 unknown15;
            public Int32 unknown16;
            public Int32 unknown17;
            public Int32 unknown18;
            public Int32 unknown19;
            public Int32 unknown20;
            public Int32 unknown21;
            public Int32 unknown22;
            public Int32 unknown23;
            public Int32 unknown24;
            public Int32 unknown25;
            public Int32 unknown26;
            public Int32 unknown27;
            public Int32 unknown28;
            public Int32 unknown29;
            public Int32 unknown30;
            public Int32 unknown31;
            public Int32 unknown32;
            public Int32 unknown33;
            public Int32 unknown34;
            public Int32 unknown35;
            public Int32 unknown36;
            public Int32 unknown37;
            public Int32 unknown38;
            public Int32 unknown39;
            public Int32 unknown40;
            public Int32 unknown41;
            public Int32 unknown42;
            public Int32 unknown43;
            public Int32 unknown44;
            public Int32 unknown45;
            public Int32 unknown46;
            public Int32 unknown47;
            public Int32 unknown48;
            public Int32 unknown49;
        }

        List<StatesTable> states;

        public States(byte[] data) : base(data) { }

        public override object GetTableArray()
        {
            return states.ToArray();
        }

        protected override void ParseTables(byte[] data)
        {
            states = ExcelTables.ReadTables<StatesTable>(data, ref offset, Count);
        }
    }
}
