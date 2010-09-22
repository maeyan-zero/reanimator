using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MonsterNamesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        [ExcelOutput(SortAscendingID = 2)]
        public Int32 code;
        [ExcelFile.ExcelOutput(IsStringID = true, TableStringId = "Strings_Names")]
        public Int32 stringKey;//stridx
        public Int32 monsterNameType;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 isNameOverride;//bool
    }
}