﻿using System;
using System.Runtime.InteropServices;
using ExcelOutput = Hellgate.ExcelFile.OutputAttribute;
using TableHeader = Hellgate.ExcelFile.TableHeader;

namespace Hellgate.Excel
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MusicGrooveLevels
    {
        TableHeader header;
        [ExcelOutput(SortAscendingID = 1, RequiresDefault = true)]
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string name;//seems to be by name, but with an empty entry first.
        public Int32 grooveLevelType;//idx
        public Int32 musicRef;//idx
        public Int32 trackNumber;
        public Int32 volume;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        Int32[] undefined1;
        public Int32 minPlayTimeInMeasures;
        public Int32 maxPlayTimeInMeasures;
        public Int32 grooveLevelAfterMaxPlayTime;//idx
        public Int32 mixState;//idx
        [ExcelOutput(IsBool = true)]
        public Int32 isAction;//bool
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        Int32[] playStingerSets;
        public Int32 noCollide;//idx
        public Int32 forParticles;//idx
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        Int32[] undefined;
    }
}