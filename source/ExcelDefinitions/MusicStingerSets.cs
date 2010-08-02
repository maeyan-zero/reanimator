using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicStingerSetsRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//empty entry is first in index.

        public Int32 musicRef;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] stinger1Measures;
        public Int32 stinger1;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined2;
        public float stinger1Chance;
        public Int32 undefined3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] stinger2Measures;
        public Int32 stinger2;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4;
        public float stinger2Chance;
        public Int32 undefined5;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] stinger3Measures;
        public Int32 stinger3;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6;
        public float stinger3Chance;
        public Int32 undefined7;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public Int32[] stinger4Measures;
        public Int32 stinger4;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8;
        public float stinger4Chance;
        public Int32 undefined9;
    }
}