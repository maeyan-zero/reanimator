using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TextureTypesRow
    {
        ExcelFile.TableHeader header;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 backgroundPriority;
        public Int32 unitPriority;
        public Int32 particlePriority;
        public Int32 uiPriority;
        public Int32 wardrobePriority;
    }
}