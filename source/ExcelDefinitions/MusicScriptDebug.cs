using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicScriptDebugRow
    {
        ExcelFile.TableHeader header;
        [ExcelOutput(SortAscendingID = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;
        [ExcelOutput(IsIntOffset = true)]
        public Int32 script;//intptr
    }
}