using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicStingerSets
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//empty entry is first in index.
        public Int32 musicRef;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined1;
        public Int32 stinger1Measure1;
        public Int32 stinger1Measure2;
        public Int32 stinger1Measure3;
        public Int32 stinger1Measure4;
        public Int32 stinger1Measure5;
        public Int32 stinger1Measure6;
        public Int32 stinger1Measure7;
        public Int32 stinger1Measure8;
        public Int32 stinger1Measure9;
        public Int32 stinger1Measure10;
        public Int32 stinger1;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined2;
        public float stinger1Chance;
        public Int32 undefined3;
        public Int32 stinger2Measure1;
        public Int32 stinger2Measure2;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 stinger2Measure3;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        Int32[] stinger2Measures;
        public Int32 stinger2;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined4;
        public float stinger2Chance;
        public Int32 undefined5;
        public Int32 stinger3Measure1;
        public Int32 stinger3Measure2;
        public Int32 stinger3Measure3;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 stinger3Measure4;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        Int32[] stinger3Measures;
        public Int32 stinger3;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined6;
        public float stinger3Chance;
        public Int32 undefined7;
        public Int32 stinger4Measure1;
        [ExcelOutput(DebugIgnoreConstantCheck = true)]
        public Int32 stinger4Measure2;
        [ExcelOutput(ConstantValue = -1)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        Int32[] stinger4Measures;
        public Int32 stinger4;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        Int32[] undefined8;
        public float stinger4Chance;
        public Int32 undefined9;
    }
}