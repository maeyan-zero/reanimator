﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterNames
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelOutput(IsStringIndex = true)]
        public Int32 stringKey;//stridx
        public Int32 monsterNameType;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 isNameOverride;//bool
    }
}