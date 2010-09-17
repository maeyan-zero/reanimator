using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class OfferRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [ExcelOutput(IsBool = true)]
        public Int32 noDuplicates;//bool
        [ExcelOutput(IsBool = true)]
        public Int32 doNotIdentify;//bool
        public Int32 numAllowedTakes;
        public Int32 treasure0UnitType;//idx
        public Int32 treasure0;//idx
        public Int32 treasure1UnitType;//idx
        public Int32 treasure1;//idx
        public Int32 treasure2UnitType;//idx
        public Int32 treasure2;//idx
        public Int32 treasure3UnitType;//idx
        public Int32 treasure3;//idx
        public Int32 treasure4UnitType;//idx
        public Int32 treasure4;//idx
        public Int32 treasure5UnitType;//idx
        public Int32 treasure5;//idx
        public Int32 treasure6UnitType;//idx
        public Int32 treasure6;//idx
        public Int32 treasure7UnitType;//idx
        public Int32 treasure7;//idx
        public Int32 offerString;//stridx
    }
}