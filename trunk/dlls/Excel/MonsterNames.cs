using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class MonsterNames : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class MonsterNamesTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 code;
            [ExcelTable.ExcelOutput(IsStringId = true, StringTable = "Strings_Names")]
            public Int32 stringKey;//stridx
            public Int32 monsterNameType;//idx
            public Int32 isNameOverride;//bool
        }

        public MonsterNames(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<MonsterNamesTable>(data, ref offset, Count);
        }
    }
}
