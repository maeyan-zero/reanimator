using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class ConditionFunctions : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class ConditionFunctionsTable
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            Int32[] header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string function;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            byte undefined1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public Int16 undefinedBool;
            public Int16 usesMonsterClass;//bool
            public Int16 usesObjectClass;//bool
            public Int16 usesStat;//bool
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string paramText0;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string paramText1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            byte undefined2;
        }

        public ConditionFunctions(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<ConditionFunctionsTable>(data, ref offset, Count);
        }

    }
}
