using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EffectsIndexRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string name;
        public Int32 fixedFunc;//idx
        public Int32 sm_11;//idx
        public Int32 sm_20_Low;//idx
        public Int32 sm_20_High;//idx
        public Int32 sm_30;//idx
        public Int32 sm_40;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 required;
    }
}