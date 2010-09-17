using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class GameGlobals
    {
        TableHeader header;
        [ExcelAttribute(IsStringOffset = true, SortID = 1)]
        public Int32 name;
        UInt32 null01;
        public Int32 intValue;
        public Single floatValue;
    }
}