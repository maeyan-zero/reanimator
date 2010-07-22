using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsIndexRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;

        public Int32 fixedFunc;//idx
        public Int32 sm_11;//idx
        public Int32 sm_20_Low;//idx
        public Int32 sm_20_High;//idx
        public Int32 sm_30;//idx
        public Int32 sm_40;//idx
        public Int32 required;//bool
    }
}