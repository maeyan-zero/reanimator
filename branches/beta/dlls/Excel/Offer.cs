using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class Offer : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class OfferTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            public Int32 noDuplicates;//bool
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

        public Offer(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<OfferTable>(data, ref offset, Count);
        }
    }
}
