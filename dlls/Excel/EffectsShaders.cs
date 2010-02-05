using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class EffectsShaders : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class EffectsShadersTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string name;

            public Int32 indoorEffect;//idx
            public Int32 outdoorEffect;//idx
            public Int32 indoorGridEffect;//idx
            public Int32 flashLightEffect;//idx
            public Int32 noCollide;//bool
            public Int32 forParticles;//bool
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            Int32[] undefined;
        }

        public EffectsShaders(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<EffectsShadersTable>(data, ref offset, Count);
        }
    }
}
