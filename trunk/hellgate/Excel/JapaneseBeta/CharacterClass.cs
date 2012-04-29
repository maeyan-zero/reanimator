using System;
using System.Runtime.InteropServices;

namespace Hellgate.Excel.JapaneseBeta
{
    [Serializable, StructLayout(LayoutKind.Sequential, Pack = 1)]
    class CharacterClassBeta
    {
        ExcelFile.RowHeader header;
        [ExcelFile.OutputAttribute(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 maleUnit7;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 maleEnabled;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit0;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit1;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit2;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit3;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit4;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit5;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit6;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "PLAYERS")]
        public Int32 femaleUnit7;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 femaleEnabled;
        [ExcelFile.OutputAttribute(IsStringIndex = true)]
        public Int32 stringOneLetterCode;
        [ExcelFile.OutputAttribute(IsBool = true)]
        public Int32 Default;
        [ExcelFile.OutputAttribute(IsTableIndex = true, TableStringId = "ITEMS")]
        public Int32 scrapItemClassSpecial;//idx
    }
}
