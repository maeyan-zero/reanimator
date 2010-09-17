﻿using System;
using System.Runtime.InteropServices;
using ExcelAttribute = Hellgate.ExcelFile.ExcelAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class BackGroundSounds
    {
        TableHeader header;
        [ExcelAttribute(SortID = 1)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public String name;
        public Int32 sounds2D0;
        public Int32 sounds2D1;
        public Int32 sounds2D2;
        public Int32 sounds2D3;
        public Int32 sounds2D4;
        public Int32 sounds2D5;
        public Int32 sounds2D6;
        public Int32 sounds2D7;
        public Int32 sounds2D8;
        public Int32 sounds2D9;
        public Int32 sounds3D0;
        public Int32 sounds3D1;
        public Int32 sounds3D2;
        public Int32 sounds3D3;
        public Int32 sounds3D4;
        public Int32 sounds3D5;
        public Int32 sounds3D6;
        public Int32 sounds3D7;
        public Int32 sounds3D8;
        public Int32 sounds3D9;
    }
}