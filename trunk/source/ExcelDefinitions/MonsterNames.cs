using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterNamesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(SortId = 2)]
        public Int32 code;
        [ExcelFile.ExcelOutput(IsStringId = true, Table = "Strings_Names")]
        public Int32 stringKey;//stridx
        public Int32 monsterNameType;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 isNameOverride;//bool
    }
}