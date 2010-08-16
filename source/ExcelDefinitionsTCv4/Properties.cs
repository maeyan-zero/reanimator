using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PropertiesTCv4Row
    {
        ExcelFile.TableHeader header;

        public Int32 property;
    }
}