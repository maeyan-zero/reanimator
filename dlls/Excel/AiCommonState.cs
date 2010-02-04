using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class AiCommonState : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class AiCommonStateTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string stat;
            public short code;
    
}

        public AiCommonState(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<AiCommonStateTable>(data, ref offset, Count);
        }

        public string GetStringFromId(int id)
        {
            foreach (AiCommonStateTable aiCommonStateTable in tables)
            {
                if (aiCommonStateTable.code == id)
                {
                    return aiCommonStateTable.stat;
                }
            }

            return "NOT FOUND";
        }
    }
}
