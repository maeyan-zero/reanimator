﻿using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class CharacterClass
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        public Int32 maleUnit0;
        public Int32 maleUnit1;
        public Int32 maleUnit2;
        public Int32 maleUnit3;
        public Int32 maleUnit4;
        public Int32 maleUnit5;
        public Int32 maleUnit6;
        public Int32 maleUnit7;
        [ExcelAttribute(IsBool = true)]
        public Int32 maleEnabled;
        public Int32 femaleUnit0;
        public Int32 femaleUnit1;
        public Int32 femaleUnit2;
        public Int32 femaleUnit3;
        public Int32 femaleUnit4;
        public Int32 femaleUnit5;
        public Int32 femaleUnit6;
        public Int32 femaleUnit7;
        [ExcelAttribute(IsBool = true)]
        public Int32 femaleEnabled;
        public Int32 unitVersionToGetSkillRespec;
        [ExcelAttribute(IsStringIndex = true)]
        public Int32 stringOneLetterCode;
        [ExcelAttribute(IsBool = true)]
        public Int32 Default;
        public Int32 scrapItemClassSpecial;//idx
    }
}