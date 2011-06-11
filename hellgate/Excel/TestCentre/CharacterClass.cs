using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;

namespace Hellgate.Excel.TestCentre
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class CharacterClassTCv4
    {
        ExcelFile.RowHeader header;

        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit7;
        [ExcelOutput(IsBool = true)]
        public Int32 maleEnabled;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit0;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit1;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit2;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit3;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit4;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit5;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit6;
        [ExcelOutput(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit7;
        [ExcelOutput(IsBool = true)]
        public Int32 femaleEnabled;
        //insert new
        //public Int32 unitVersionToGetSkillRespec;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringOneLetterCode;
        [ExcelOutput(IsBool = true)]
        public Int32 Default;
        [ExcelOutput(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 scrapItemClassSpecial;//idx
    }
}