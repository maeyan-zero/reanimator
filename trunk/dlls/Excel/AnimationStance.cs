using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class AnimationStance : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AnimationStanceTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 dontChangeFrom;//bool
        }

        public AnimationStance(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AnimationStanceTable>(data, ref offset, Count);
        }
    }
}
