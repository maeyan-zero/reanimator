using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ProcsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;

        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 verticalCenter;//bool
        public float coolDownInSeconds;
        public Int32 targetInstrumentOwner;//a single bit
        public float delayeProcTimeInSeconds;
        public Int32 skill1;//idx
        public Int32 skill1Param;//idx
        public Int32 skill2;//idx
        public Int32 skill2Param;//idx
    }
}