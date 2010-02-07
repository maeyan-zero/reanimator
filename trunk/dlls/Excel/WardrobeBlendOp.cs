using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobeBlendOp : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeBlendOpTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 replaceAllParts;//bool;
            public Int32 noTextureChange;//bool;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] removeParts;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] addParts;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
            public string blend;
            public short undefined1;
            public Int32 undefined2;
            public Int32 target;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] covers;
        }

        public WardrobeBlendOp(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeBlendOpTable>(data, ref offset, Count);
        }
    }
}
