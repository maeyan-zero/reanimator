using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class WardrobeBody : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class WardrobeBodyTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public Int32[] layers;
            public Int32 Base;//idx
            public Int32 head;//idx
            public Int32 hair;//idx
            public Int32 facialHair;//idx
            public Int32 skin;//idx
            public Int32 hairColorTexture;//idx
            public byte skinColor;
            public byte hairColor;
            public short undefined;
            public Int32 randomize;//bool
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public Int32[] randomLayerSets;
        }

        public WardrobeBody(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<WardrobeBodyTable>(data, ref offset, Count);
        }
    }
}
