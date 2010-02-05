using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MaterialsCollision : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MaterialsCollisionTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 materialNumber;
            public Int32 floor;//bool
            public Int32 mapsTo;//idx
            public float directOcclusion;
            public float reverbOcclusion;
            public Int32 debugColor;//idx
        }

        public MaterialsCollision(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MaterialsCollisionTable>(data, ref offset, Count);
        }
    }
}
