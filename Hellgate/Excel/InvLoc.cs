using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class InvLocRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string name;
        [ExcelOutput(SortId = 2)]
        public short code;
    }
}