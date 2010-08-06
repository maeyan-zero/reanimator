using System;
using System.Runtime.InteropServices;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeModelRow
    {
        ExcelFile.TableHeader header;

        Int32 undefined1;
        public Int32 modelGroup;//idx;
        public Int32 appearanceGroup;//idx;
        public Int32 appearanceGroup2;//idx;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string fileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string defaultMaterial;
        Int32 undefined3;
        public Int32 partGroup;
        Int32 undefinedBool1;
        Int32 undefinedBool2;
        Int32 undefinedBool3;
        Int32 undefined4;
        Int32 undefined5;
        public float boxMinX;
        public float boxMinY;
        public float boxMinZ;
        public float boxMaxX;
        public float boxMaxY;
        public float boxMaxZ;
    }
}