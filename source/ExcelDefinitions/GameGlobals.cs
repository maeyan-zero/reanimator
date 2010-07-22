using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        class GameGlobalsRow
        {
            ExcelFile.TableHeader header;

            [ExcelFile.ExcelOutputAttribute(IsStringOffset = true)]
            public Int32 name;
            Int32 _buffer;
            public Int32 intValue;
            public float floatValue;
        }
}