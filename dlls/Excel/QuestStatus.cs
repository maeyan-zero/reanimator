using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class QuestStatus : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class QuestStatusTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            public Int32 code;
            public Int32 isGood;//bool

        }
        public QuestStatus(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<QuestStatusTable>(data, ref offset, Count);
        }
    }
}