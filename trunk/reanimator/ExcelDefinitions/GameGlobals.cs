using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class GameGlobalsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(IsStringOffset = true, SortAscendingID = 1)]
        public Int32 name;
        UInt32 _buffer;
        public Int32 intValue;
        public float floatValue;
    }
}