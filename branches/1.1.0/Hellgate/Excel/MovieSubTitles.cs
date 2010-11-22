﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Reanimator.ExcelFile.ExcelOutputAttribute;

namespace Reanimator.ExcelDefinitions
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MovieSubTitlesRow
    {
        ExcelFile.TableHeader header;

        [ExcelOutput(SortId = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;

        public Int32 movie0;
        public Int32 movie1;
        public Int32 movie2;
        public Int32 movie3;
        public Int32 language;//idx
        public Int32 String;//stridx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined;
    }
}