using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PropertiesRow
    {
        ExcelFile.TableHeader header;

        public Int32 property;
    }
}