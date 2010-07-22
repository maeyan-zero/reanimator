using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsShadersRow
    {
        ExcelFile.TableHeader header;

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
}