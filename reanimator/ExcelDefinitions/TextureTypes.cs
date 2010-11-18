using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TextureTypesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortAscendingID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 backgroundPriority;
        public Int32 unitPriority;
        public Int32 particlePriority;
        public Int32 uiPriority;
        public Int32 wardrobePriority;
    }
}