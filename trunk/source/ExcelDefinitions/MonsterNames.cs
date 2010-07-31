﻿using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterNamesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 code;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Names")]
        public Int32 stringKey;//stridx
        public Int32 monsterNameType;//idx
        public Int32 isNameOverride;//bool
    }
}