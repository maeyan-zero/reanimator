using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Reanimator.Excel
{
    public class CharacterClass : ExcelTable
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class CharacterClassTable
        {
            TableHeader header;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string name;

            public Int32 maleUnit0;
            public Int32 maleUnit1;
            public Int32 maleUnit2;
            public Int32 maleUnit3;
            public Int32 maleUnit4;
            public Int32 maleUnit5;
            public Int32 maleUnit6;
            public Int32 maleUnit7;
            public Int32 maleEnabled;//bool
            public Int32 femaleUnit0;
            public Int32 femaleUnit1;
            public Int32 femaleUnit2;
            public Int32 femaleUnit3;
            public Int32 femaleUnit4;
            public Int32 femaleUnit5;
            public Int32 femaleUnit6;
            public Int32 femaleUnit7;
            public Int32 femaleEnabled;//bool
            public Int32 unitVersionToGetSkillRespec;
            public Int32 stringOneLetterCode;//stridx
            public Int32 Default;//bool
            public Int32 scrapItemClassSpecial;//idx
        }

        public CharacterClass(byte[] data) : base(data) { }

        protected override void ParseTables(byte[] data)
        {
            ReadTables<CharacterClassTable>(data, ref offset, Count);
        }
    }
}
