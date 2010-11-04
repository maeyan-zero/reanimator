using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class Offer
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
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
