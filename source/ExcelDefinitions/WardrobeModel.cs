using System;
using System.Runtime.InteropServices;
using TableHeader = Reanimator.ExcelFile.TableHeader;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class WardrobeModelRow
    {
        TableHeader header;
        public Int32 undefined1;
        [ExcelOutput(SortAscendingID = 1, SortColumnTwo = "appearanceGroup", ExcludeZero = true)]
        public Int32 modelGroup;//idx;
        public Int32 appearanceGroup;//idx;
        public Int32 appearanceGroup2;//idx;
        public Int32 undefined2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string fileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string defaultMaterial;
        public Int32 undefined3;
        public Int32 partGroup;
        public Int32 undefinedBool1;
        public Int32 undefinedBool2;
        public Int32 undefinedBool3;
        public Int32 undefined4;
        public Int32 undefined5;
        public float boxMinX;
        public float boxMinY;
        public float boxMinZ;
        public float boxMaxX;
        public float boxMaxY;
        public float boxMaxZ;
    }
}