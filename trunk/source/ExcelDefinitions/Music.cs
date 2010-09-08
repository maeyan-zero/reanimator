using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicRow
    {
        ExcelFile.TableHeader header;
        [ExcelOutput(SortId = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//this seems to be unique names, since it starts with an empty entry, like similar indices
        public Int32 undefined1;
        public Int32 baseCondition;//idx
        public Int32 musicRef;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined2;
    }
}