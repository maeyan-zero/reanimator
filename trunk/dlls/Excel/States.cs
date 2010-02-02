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
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 stringId;
            Int32 buffer;
            public Int32 id;
            Int32 buffer1;              // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 effect;
            Int32 buffer2;              // always 0
            public Int32 unknown4;
            public Int32 unknown5;
            public Int32 unknown6;
            public Int32 unknown7;
            public Int32 unknown8;
            public Int32 unknown9;
            public Int32 unknown10;
            Int32 unknown11;            // always -1
            Int32 unknown12;            // always -1
            Int32 unknown13;            // always -1
            public Int32 unknown14;
            public Int32 unknown15;
            public Int32 unknown16;
            public Int32 unknown17;
            Int32 unknown18;            // always 0
            public Int32 unknown19;
            public Int32 unknown20;
            Int32 unknown21;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            Int32 unknown22;
            Int32 unknown23;            // always 0
            Int32 unknown24;            // always 0
            Int32 unknown25;            // always 0
            Int32 unknown26;            // always 0
            Int32 unknown27;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown28;
            Int32 unknown29;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown30;
            Int32 unknown31;            // always 0
            public Int32 unknown32;
            Int32 unknown33;            // always 0
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown34;
            Int32 unknown35;            // always 0
            public Int32 unknown36;
            Int32 unknown37;            // always 0
            Int32 unknown38;            // always 0
            public Int32 unknown39;
            public Int32 unknown40;
            public Int32 unknown41;
            Int32 unknown42;            // always -1
            Int32 unknown43;            // always -1
            Int32 unknown44;            // always -1
            Int32 unknown45;            // always 0
            public Int32 unknown46;
            [ExcelTables.ExcelOutput(IsStringOffset = true)]
            public Int32 unknown47;
            Int32 unknown48;            // always 0
            Int32 unknown49;            // always 0
        }

        public States(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<StatesTable>(data, ref offset, Count);
        }
    }
}
