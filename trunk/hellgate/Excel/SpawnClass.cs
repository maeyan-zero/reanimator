using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using RowHeader = Hellgate.ExcelFile.RowHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SpawnClass
    {
        RowHeader header;
        [ExcelOutput(SortColumnOrder = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string spawnClass;
        [ExcelOutput(SortColumnOrder = 2)]
        public Int32 code;
        public Int32 pickType;
        [ExcelOutput(IsBool = true)]
        public Int32 rememberPick;//bool
        public Int32 pick1;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn1;
        [ExcelOutput(IsScript = true)]
        public Int32 count1;//intptr
        Int32 undefined1b;
        public Int32 weight1;
        public Int32 pick2;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn2;
        [ExcelOutput(IsScript = true)]
        public Int32 count2;//intptr
        Int32 undefined2b;
        public Int32 weight2;
        public Int32 pick3;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn3;
        [ExcelOutput(IsScript = true)]
        public Int32 count3;//intptr
         Int32 undefined3b;
        public Int32 weight3;
        public Int32 pick4;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn4;
        [ExcelOutput(IsScript = true)]
        public Int32 count4;//intptr
        Int32 undefined4b;
        public Int32 weight4;
        public Int32 pick5;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn5;
        [ExcelOutput(IsScript = true)]
        public Int32 count5;//intptr
        Int32 undefined5b;
        public Int32 weight5;
        public Int32 pick6;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn6;
        [ExcelOutput(IsScript = true)]
        public Int32 count6;//intptr
        Int32 undefined6b;
        public Int32 weight6;
        public Int32 pick7;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn7;
        [ExcelOutput(IsScript = true)]
        public Int32 count7;//intptr
        Int32 undefined7b;
        public Int32 weight7;
        public Int32 pick8;//1 is specific monster, 3 is another spawn class entry.
        public Int32 spawn8;
        [ExcelOutput(IsScript = true)]
        public Int32 count8;//intptr
        Int32 undefined8b;
        public Int32 weight8;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        Int32[] undefined9;
    }
}
